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
        var lists = document.querySelectorAll('.q-list');
        function apply() { lists.forEach(function (l) { l.setAttribute('data-answers', toggle.checked ? 'shown' : 'hidden'); }); }
        toggle.checked = store.get('nx-answers') !== 'hidden';
        toggle.addEventListener('change', function () { store.set('nx-answers', toggle.checked ? 'shown' : 'hidden'); apply(); });
        apply();
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
