/* Nexam quiz taking page (vanilla JS).
   Expects: .questions-container[data-id=QuizID] > [data-question][data-id=QuestionID][data-correct=answer],
   progress elements [data-progress-bar] / [data-progress-text] / [data-progress-text-footer], and #btnSave. */
(function () {
    'use strict';

    var container = document.querySelector('.questions-container');
    var saveButton = document.getElementById('btnSave');
    if (!container || !saveButton) return;

    var cards = Array.prototype.slice.call(container.querySelectorAll('[data-question]'));
    var bar = document.querySelector('[data-progress-bar]');
    var barFill = bar ? bar.firstElementChild : null;
    var labels = document.querySelectorAll('[data-progress-text], [data-progress-text-footer]');
    var buttonHtml = saveButton.innerHTML;
    var submitted = false;

    function escapeHtml(s) {
        return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    /* The chosen option's value, or what was typed for questions without choices. */
    function answerOf(card) {
        var checked = card.querySelector('input[type="radio"]:checked');
        if (checked) return checked.value.trim();
        var text = card.querySelector('input[type="text"]');
        return text ? text.value.trim() : '';
    }

    function answeredCount() {
        return cards.filter(function (c) { return answerOf(c) !== ''; }).length;
    }

    function updateProgress() {
        var done = answeredCount();
        var total = cards.length;
        if (barFill) barFill.style.width = (total ? Math.round(done * 100 / total) : 0) + '%';
        if (bar) bar.setAttribute('aria-valuenow', String(done));
        labels.forEach(function (l) { l.textContent = done + ' of ' + total + ' answered'; });
    }

    container.addEventListener('change', function (e) {
        var card = e.target.closest('[data-question]');
        if (card) card.classList.remove('is-missing');
        updateProgress();
    });
    container.addEventListener('input', function (e) {
        if (e.target.matches('input[type="text"]')) {
            var card = e.target.closest('[data-question]');
            if (card) card.classList.remove('is-missing');
            updateProgress();
        }
    });

    function setBusy(busy) {
        saveButton.disabled = busy;
        saveButton.innerHTML = busy ? 'Submitting&hellip;' : buttonHtml;
    }

    function showError(message, retry) {
        NexamDialog.show({
            title: 'Sorry!',
            html: '<p>' + escapeHtml(message) + '</p>',
            actions: [{ label: retry ? 'Try again' : 'Close', primary: true }]
        });
    }

    function showResult(result) {
        var html = '<p class="score">' + escapeHtml(result.Score) + '</p>';
        if (result.CorrectAnswers != null && result.TotalQuestions != null) {
            html += '<p class="muted">You answered ' + escapeHtml(result.CorrectAnswers) + ' of ' +
                escapeHtml(result.TotalQuestions) + ' questions correctly.</p>';
        }
        NexamDialog.show({
            title: (result.Remarks || 'Quiz complete') + ', ' + (result.Name || 'student') + '!',
            html: html,
            persistent: true,
            actions: [{ label: 'View my results', href: '/User/UserDashboard', primary: true }]
        });
    }

    function submit() {
        if (submitted) return;

        var missing = cards.filter(function (c) { return answerOf(c) === ''; });
        cards.forEach(function (c) { c.classList.toggle('is-missing', missing.indexOf(c) !== -1); });
        if (missing.length) {
            var first = missing[0];
            NexamDialog.show({
                title: 'Please answer all questions',
                html: '<p>' + missing.length + (missing.length === 1 ? ' question is' : ' questions are') + ' still unanswered.</p>',
                actions: [{
                    label: 'Go to first unanswered',
                    primary: true,
                    onClick: function () {
                        first.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        var field = first.querySelector('input');
                        if (field) field.focus({ preventScroll: true });
                    }
                }]
            });
            return;
        }

        var payload = {
            QuizID: Number(container.getAttribute('data-id')),
            QuizQuestions: cards.map(function (card) {
                var userAnswer = answerOf(card);
                var correctAnswer = (card.getAttribute('data-correct') || '').trim();
                return {
                    QuestionID: Number(card.getAttribute('data-id')),
                    UserAnswer: userAnswer,
                    CorrectAnswer: correctAnswer,
                    IsCorrect: userAnswer === correctAnswer
                };
            })
        };

        setBusy(true);
        fetch('/Quiz/InsertQuizQuestion', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8', 'Accept': 'application/json' },
            credentials: 'same-origin',
            body: JSON.stringify(payload)
        }).then(function (response) {
            if (response.status === 401) {
                setBusy(false);
                NexamDialog.show({
                    title: 'Please log in again',
                    html: '<p>Your session has expired. Log in and start the quiz again to save your result.</p>',
                    actions: [{ label: 'Log in', href: '/User/UserLogin', primary: true }]
                });
                return null;
            }
            if (!response.ok) throw new Error('HTTP ' + response.status);
            return response.json();
        }).then(function (result) {
            if (!result) return;
            submitted = true;
            saveButton.disabled = true;
            saveButton.innerHTML = 'Submitted';
            showResult(result);
        }).catch(function () {
            setBusy(false);
            showError('We could not submit your quiz. Please check your connection and try again.', true);
        });
    }

    saveButton.addEventListener('click', submit);
    updateProgress();
})();
