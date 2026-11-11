/* Nexam UI behaviour (vanilla JS, no dependencies).
   Features: theme toggle, mobile drawer, live filters, answers switch, dialogs, "/" to search. */
(function () {
    'use strict';

    var root = document.documentElement;
    var store = {
        get: function (k) { try { return localStorage.getItem(k); } catch (e) { return null; } },
        set: function (k, v) { try { localStorage.setItem(k, v); } catch (e) { /* private mode */ } }
    };

    /* ---------- Theme ---------- */
    function applyTheme(theme) {
        root.setAttribute('data-theme', theme);
        document.querySelectorAll('[data-theme-toggle]').forEach(function (b) {
            b.setAttribute('aria-label', theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
            b.setAttribute('aria-pressed', theme === 'dark' ? 'true' : 'false');
        });
    }
    document.addEventListener('click', function (e) {
        var t = e.target.closest('[data-theme-toggle]');
        if (!t) return;
        var next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
        store.set('nx-theme', next);
        applyTheme(next);
    });
    applyTheme(root.getAttribute('data-theme') || 'light');

    /* ---------- Mobile drawer ---------- */
    var drawer = document.getElementById('drawer');
    function setDrawer(open) {
        if (!drawer) return;
        drawer.setAttribute('data-open', open ? 'true' : 'false');
        document.body.style.overflow = open ? 'hidden' : '';
        document.querySelectorAll('[data-menu-toggle]').forEach(function (b) {
            b.setAttribute('aria-expanded', open ? 'true' : 'false');
        });
    }
    document.addEventListener('click', function (e) {
        if (e.target.closest('[data-menu-toggle]')) { setDrawer(drawer.getAttribute('data-open') !== 'true'); return; }
        if (drawer && e.target.closest('#drawer a')) setDrawer(false);
    });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') { setDrawer(false); closeDialog(); }
        // "/" focuses the search box (unless the user is typing somewhere)
        if (e.key === '/' && !/^(INPUT|TEXTAREA|SELECT)$/.test((document.activeElement || {}).tagName || '')) {
            var s = document.querySelector('[data-search-input]');
            if (s && s.offsetParent !== null) { e.preventDefault(); s.focus(); }
        }
    });
    window.addEventListener('resize', function () { if (window.innerWidth >= 1024) setDrawer(false); });

    /* ---------- Live filters ----------
       <div data-filter-scope>
         <input data-filter-input>  <button data-filter-chip="2026">  (chip value "" = All)
         <span data-filter-count>   <div data-filter-empty class="hidden">
         items: [data-filter-item] with optional data-filter-group (matched against the chip) */
    function initFilter(scope) {
        var input = scope.querySelector('[data-filter-input]');
        var chips = Array.prototype.slice.call(scope.querySelectorAll('[data-filter-chip]'));
        var items = Array.prototype.slice.call(scope.querySelectorAll('[data-filter-item]'));
        var count = scope.querySelector('[data-filter-count]');
        var empty = scope.querySelector('[data-filter-empty]');
        if (!items.length) return;

        var texts = items.map(function (i) { return (i.getAttribute('data-filter-text') || i.textContent).toLowerCase(); });
        var group = '';
        var timer = null;

        function run() {
            var q = input ? input.value.trim().toLowerCase() : '';
            var terms = q ? q.split(/\s+/) : [];
            var shown = 0;
            for (var n = 0; n < items.length; n++) {
                var ok = true;
                if (group && items[n].getAttribute('data-filter-group') !== group) ok = false;
                for (var t = 0; ok && t < terms.length; t++) if (texts[n].indexOf(terms[t]) === -1) ok = false;
                items[n].hidden = !ok;
                if (ok) shown++;
            }
            if (count) count.textContent = 'Showing ' + shown + ' of ' + items.length;
            if (empty) empty.classList.toggle('hidden', shown !== 0);
        }

        if (input) input.addEventListener('input', function () { clearTimeout(timer); timer = setTimeout(run, 120); });
        chips.forEach(function (chip) {
            chip.addEventListener('click', function () {
                group = chip.getAttribute('data-filter-chip') || '';
                chips.forEach(function (c) { c.setAttribute('aria-pressed', c === chip ? 'true' : 'false'); });
                run();
            });
        });
        run();
    }
    document.querySelectorAll('[data-filter-scope]').forEach(initFilter);

    /* ---------- Confirm before a destructive link/button:  <a data-confirm="Delete this question?" ...> ---------- */
    document.addEventListener('click', function (e) {
        var el = e.target.closest('[data-confirm]');
        if (el && !window.confirm(el.getAttribute('data-confirm'))) e.preventDefault();
    });

    /* ---------- Show / hide answers on question lists ---------- */
    document.querySelectorAll('[data-answers-toggle]').forEach(function (toggle) {
        function apply() { document.querySelectorAll('.q-list').forEach(function (l) { l.setAttribute('data-answers', toggle.checked ? 'shown' : 'hidden'); }); }
        toggle.checked = store.get('nx-answers') !== 'hidden';
        toggle.addEventListener('change', function () { store.set('nx-answers', toggle.checked ? 'shown' : 'hidden'); apply(); });
        toggle.addEventListener('nx:refresh', apply);   // fired after live-search swaps the results
        apply();
    });

    /* ---------- Live search ----------
       <form data-live-search> ... <div data-live-results> ... </div>
       Re-runs the form's GET request as the user types (or changes a select) and swaps only the results. */
    document.querySelectorAll('form[data-live-search]').forEach(function (form) {
        var box = document.querySelector('[data-live-results]');
        if (!box || !window.fetch) return;
        var timer = null, ctl = null, last = null;

        function url() {
            var qs = new URLSearchParams(new FormData(form)).toString();
            return form.getAttribute('action') + (qs ? '?' + qs : '');
        }
        function run() {
            var u = url();
            if (u === last) return;
            last = u;
            if (ctl) ctl.abort();
            ctl = window.AbortController ? new AbortController() : null;
            box.setAttribute('aria-busy', 'true');
            fetch(u, { headers: { 'X-Requested-With': 'fetch' }, signal: ctl ? ctl.signal : undefined })
                .then(function (r) { if (!r.ok) throw new Error(r.status); return r.text(); })
                .then(function (html) {
                    var next = new DOMParser().parseFromString(html, 'text/html').querySelector('[data-live-results]');
                    if (!next) { window.location.href = u; return; }
                    box.innerHTML = next.innerHTML;
                    box.removeAttribute('aria-busy');
                    box.querySelectorAll('[data-filter-scope]').forEach(initFilter);
                    try { history.replaceState(null, '', u); } catch (e) { /* ignore */ }
                    document.querySelectorAll('[data-answers-toggle]').forEach(function (t) { t.dispatchEvent(new Event('nx:refresh')); });
                })
                .catch(function (err) { if (err && err.name === 'AbortError') return; box.removeAttribute('aria-busy'); last = null; });
        }

        form.addEventListener('input', function (e) {
            if (e.target.matches('[data-answers-toggle]')) return;
            clearTimeout(timer);
            timer = setTimeout(run, 300);
        });
        form.addEventListener('change', function (e) { if (e.target.tagName === 'SELECT') { clearTimeout(timer); run(); } });
        form.addEventListener('submit', function (e) { e.preventDefault(); clearTimeout(timer); run(); });
        last = url();
    });


    /* ---------- Search suggestions (type-ahead) ----------
       <form data-suggest="/Home/SearchSuggest" action="/Home/SearchResult"> <input name="search"> [<select name="subject">] </form>
       Shows the top matches under the box as you type; Enter (or "See all") opens the full results page. */
    document.querySelectorAll('form[data-suggest]').forEach(function (form) {
        var input = form.querySelector('[name=search]');
        if (!input || !window.fetch) return;
        var endpoint = form.getAttribute('data-suggest');
        var panel = document.createElement('div');
        panel.className = 'suggest';
        panel.setAttribute('role', 'listbox');
        panel.hidden = true;
        if (getComputedStyle(form).position === 'static') form.style.position = 'relative';
        form.appendChild(panel);
        input.setAttribute('autocomplete', 'off');
        input.setAttribute('aria-autocomplete', 'list');

        var timer = null, ctl = null, active = -1, seq = 0;
        function close() { panel.hidden = true; active = -1; }
        function rows() { return panel.querySelectorAll('a'); }
        function mark(i) {
            var r = rows();
            if (!r.length) return;
            active = (i + r.length) % r.length;
            r.forEach(function (a, n) { a.setAttribute('aria-selected', n === active ? 'true' : 'false'); });
            r[active].scrollIntoView({ block: 'nearest' });
        }
        function allUrl(q) {
            var p = new URLSearchParams(new FormData(form));
            p.set('search', q);
            if (p.get('subject') === '') p.delete('subject');
            return form.getAttribute('action') + '?' + p.toString();
        }
        function render(data, q) {
            panel.textContent = '';
            if (!data.items || !data.items.length) {
                var none = document.createElement('div');
                none.className = 'suggest-empty';
                none.textContent = 'No questions found for “' + q + '”';
                panel.appendChild(none);
            } else {
                data.items.forEach(function (it) {
                    var a = document.createElement('a');
                    a.href = it.url;
                    a.setAttribute('role', 'option');
                    var t = document.createElement('span');
                    t.className = 'suggest-text';
                    t.setAttribute('dir', 'auto');
                    t.textContent = it.text;
                    a.appendChild(t);
                    if (it.subject) {
                        var s = document.createElement('span');
                        s.className = 'suggest-sub';
                        s.textContent = it.subject;
                        a.appendChild(s);
                    }
                    panel.appendChild(a);
                });
                var all = document.createElement('a');
                all.className = 'suggest-all';
                all.href = allUrl(q);
                all.textContent = 'See all ' + data.total + (data.total === 1 ? ' result' : ' results') + ' →';
                panel.appendChild(all);
            }
            active = -1;
            panel.hidden = false;
        }
        function run() {
            var q = input.value.trim();
            if (q.length < 2) { close(); return; }
            var p = new URLSearchParams(new FormData(form));
            p.set('search', q);
            if (ctl) ctl.abort();
            ctl = window.AbortController ? new AbortController() : null;
            var mine = ++seq;
            fetch(endpoint + '?' + p.toString(), { signal: ctl ? ctl.signal : undefined, headers: { 'Accept': 'application/json' } })
                .then(function (r) { if (!r.ok) throw new Error(r.status); return r.json(); })
                .then(function (d) { if (mine === seq && document.activeElement === input) render(d, q); })
                .catch(function () { /* aborted or offline: keep whatever is shown */ });
        }
        input.addEventListener('input', function () { clearTimeout(timer); timer = setTimeout(run, 250); });
        input.addEventListener('focus', function () { if (panel.children.length && input.value.trim().length >= 2) panel.hidden = false; });
        form.addEventListener('change', function (e) { if (e.target.tagName === 'SELECT') run(); });
        input.addEventListener('keydown', function (e) {
            if (panel.hidden) return;
            if (e.key === 'ArrowDown') { e.preventDefault(); mark(active + 1); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); mark(active - 1); }
            else if (e.key === 'Enter' && active >= 0) { e.preventDefault(); rows()[active].click(); }
            else if (e.key === 'Escape') { close(); }
        });
        document.addEventListener('click', function (e) { if (!form.contains(e.target)) close(); });
        form.addEventListener('focusout', function () { setTimeout(function () { if (!form.contains(document.activeElement)) close(); }, 0); });
    });
    /* ---------- Dialog ----------
       NexamDialog.show({ title, html, actions: [{ label, href, primary, onClick }] }) */
    var current = null;
    function closeDialog() { if (current) { current.remove(); current = null; } }
    window.NexamDialog = {
        close: closeDialog,
        show: function (o) {
            closeDialog();
            var back = document.createElement('div');
            back.className = 'dialog-backdrop';
            back.setAttribute('role', 'dialog');
            back.setAttribute('aria-modal', 'true');
            var box = document.createElement('div');
            box.className = 'dialog';
            var h = document.createElement('h2');
            h.textContent = o.title || '';
            var body = document.createElement('div');
            body.innerHTML = o.html || '';
            var actions = document.createElement('div');
            actions.className = 'form-actions';
            (o.actions || [{ label: 'Close' }]).forEach(function (a) {
                var el = document.createElement(a.href ? 'a' : 'button');
                el.className = 'btn ' + (a.primary ? 'btn-primary' : 'btn-secondary');
                el.textContent = a.label;
                if (a.href) el.href = a.href;
                el.addEventListener('click', function () { if (a.onClick) a.onClick(); if (!a.href) closeDialog(); });
                actions.appendChild(el);
            });
            box.appendChild(h); box.appendChild(body); box.appendChild(actions);
            back.appendChild(box);
            back.addEventListener('click', function (e) { if (e.target === back && !o.persistent) closeDialog(); });
            document.body.appendChild(back);
            current = back;
            var focusable = box.querySelector('a,button'); if (focusable) focusable.focus();
        }
    };
})();
