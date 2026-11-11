/* Forum moderation (admin): publish / delete a forum question.
   Buttons carry data-action="publish|delete" and data-id="<AskQuestionID>".
   Endpoints return JSON true/false. Uses NexamDialog from nexam.js. */
(function () {
    'use strict';

    var ACTIONS = {
        publish: {
            url: '/Forum/PublishQuestionByAdmin',
            ok: 'Your Question has been Published.',
            fail: "We can't publish your question, Please try again.",
            error: "We can't publish your question due to internal error."
        },
        'delete': {
            url: '/Forum/DeleteQuestions',
            ok: 'Your Question has been Deleted.',
            fail: "We can't Deleted your question, Please try again.",
            error: "We can't Deleted your question due to internal error."
        }
    };
    var NEXT = '/Forum/UnPublish';

    function popup(ok, message) {
        var p = document.createElement('p');
        p.textContent = message;
        NexamDialog.show({
            title: ok ? 'Thank you!' : 'Sorry!',
            html: p.outerHTML,
            persistent: true,
            actions: [{ label: 'Continue', href: NEXT, primary: true }]
        });
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
                    if (btn.getAttribute('data-action') === 'delete') {
                        var row = btn.closest('tr');
                        if (row) row.hidden = true;
                    }
                    popup(true, cfg.ok);
                } else {
                    btn.disabled = false;
                    popup(false, cfg.fail);
                }
            })
            .catch(function () {
                btn.disabled = false;
                popup(false, cfg.error);
            });
    });
})();
