using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class QuizService(IQuizRepository quizzes) : IQuizService
{
    public async Task<QuizSubmission> StartAsync(Quiz quiz)
    {
        var (quizId, questions) = await quizzes.CreateAsync(quiz);
        return new QuizSubmission { QuizID = quizId, QuestionsDOs = questions };
    }

    public async Task<QuizResult> SubmitAsync(QuizSubmission submission)
    {
        var answers = submission.QuizQuestions ?? new List<QuizQuestion>();
        var total = answers.Count;
        var correct = answers.Count(a => (a.UserAnswer ?? "").Trim() == (a.CorrectAnswer ?? "").Trim());

        // Integer percentage, as it has always been calculated.
        var score = total == 0 ? 0 : (int)Math.Ceiling((decimal)((100 * correct) / total));

        var result = new QuizResult
        {
            Score = score + "%",
            Remarks = score <= 50 ? "FAIL" : score < 70 ? "PASS" : "Excellent",
            TotalQuestions = total,
            CorrectAnswers = correct
        };

        foreach (var answer in answers)
        {
            answer.QuizID = submission.QuizID;
            await quizzes.SaveQuestionResultAsync(answer);
        }
        await quizzes.UpdateScoreAsync(submission.QuizID, score);

        return result;
    }

    public async Task<Quiz> GetByIdAsync(int quizId) =>
        new() { quizQuestionDOs = await quizzes.GetQuestionsAsync(quizId) };
}
