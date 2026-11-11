/* Forum moderation (admin): publish / delete a forum question.
   Buttons carry data-action="publish|delete" and data-id="<AskQuestionID>".
   Endpoints return JSON true/false. Success updates the table row in place; failures use NexamDialog. */
(function () {
    'use strict';

    var ACTIONS = {
        publish: {
            url: '/Forum/PublishQuestionByAdmin',
            fail: "We can't publish your question, Please try again.",
            error: "We can't publish your question due to internal error."
        },
        'delete': {
            url: '/Forum/DeleteQuestions',
            fail: "We can't Deleted your question, Please try again.",
            error: "We can't Deleted your question due to internal error."
        }
    };
    // Success is silent: the row is updated or removed in place. Only failures show a dialog.
    function fail(message) {
        var p = document.createElement('p');
        p.textContent = message;
        NexamDialog.show({ title: 'Sorry!', html: p.outerHTML, actions: [{ label: 'Close', primary: true }] });
    }

    function renumber(body) {
        if (!body) return;
        var n = 1;
        body.querySelectorAll('tr').forEach(function (r) { var c = r.querySelector('td.num'); if (c) c.textContent = n++; });
    }

    // Delete, or publish from the review queue, removes the row; publishing elsewhere flips the status badge.
    function applyResult(btn, action) {
        var row = btn.closest('tr');
        var scope = btn.closest('[data-filter-scope]');
        if (!row) return;
        if (action === 'delete' || location.pathname.toLowerCase().indexOf('/forum/unpublish') === 0) {
            var body = row.parentNode;
            row.remove();
            renumber(body);
        } else {
            var badge = row.querySelector('.badge');
            if (badge) { badge.className = 'badge badge-success'; badge.textContent = 'Published'; }
            btn.remove();
        }
        if (scope) scope.dispatchEvent(new Event('nx:refresh'));
    }

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('[data-action][data-id]');
        if (!btn || e.defaultPrevented) return;      // defaultPrevented: the user cancelled a data-confirm prompt
        var cfg = ACTIONS[btn.getAttribute('data-action')];
        if (!cfg) return;
        e.preventDefault();
        if (btn.disabled) return;
        btn.disabled = true;

        fetch(cfg.url + '?QuestionID=' + encodeURIComponent(btn.getAttribute('data-id')), {
            method: 'GET',
            credentials: 'same-origin',
            headers: { 'Accept': 'application/json' }
        })
            .then(function (r) { if (!r.ok) throw new Error('HTTP ' + r.status); return r.json(); })
            .then(function (result) {
                if (result === true) {
                    applyResult(btn, btn.getAttribute('data-action'));
                } else {
                    btn.disabled = false;
                    fail(cfg.fail);
                }
            })
            .catch(function () {
                btn.disabled = false;
                fail(cfg.error);
            });
    });
})();
