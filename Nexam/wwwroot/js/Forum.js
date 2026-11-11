/* Nexam forum: "Ask a question" page (vanilla JS).
   Sends the question to /Forum/SaveQuestion (GET, XMLHttpRequest header, JSON true/false reply). */
(function () {
    'use strict';

    var form = document.getElementById('askForm');
    var button = document.getElementById('questionSubmit');
    if (!form || !button) return;

    var buttonHtml = button.innerHTML;

    function value(id) {
        var el = document.getElementById(id);
        return el ? el.value.trim() : '';
    }

    function fail(message) {
        NexamDialog.show({
            title: 'Sorry!',
            html: '<p>' + message + '</p>',   // static messages only
            actions: [{ label: 'Try again', primary: true }]
        });
    }

    form.addEventListener('submit', function (e) {
        // The browser's own validation has already run; without JS the form posts normally.
        e.preventDefault();

        var params = new URLSearchParams({
            uname: value('qname'),
            qemail: value('qemail'),
            qcountry: value('qcountry'),
            qsubject: value('qsubject'),
            qexplanation: value('qexplanation')
        });

        button.disabled = true;
        button.textContent = 'Sending...';

        fetch('/Forum/SaveQuestion?' + params.toString(), {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest', 'Accept': 'application/json' },
            credentials: 'same-origin'
        }).then(function (response) {
            if (!response.ok) throw new Error('HTTP ' + response.status);
            return response.json();
        }).then(function (saved) {
            if (saved === true) {
                form.reset();
                NexamDialog.show({
                    title: 'Thank you!',
                    html: '<p>We\'ll review your question and publish it very soon.</p>',
                    persistent: true,
                    actions: [{ label: 'Go to forum', href: '/Nexam-Forum', primary: true }]
                });
            } else {
                fail('We can\'t publish your question, Please try again.');
            }
        }).catch(function () {
            fail('We can\'t publish your question due to internal error.');
        }).then(function () {
            button.disabled = false;
            button.innerHTML = buttonHtml;
        });
    });
})();
