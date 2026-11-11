using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Controllers;

public class QuizController(IQuizService quizzes, ISubjectService subjects) : Controller
{
    public IActionResult QuizHome() => View();

    /// <summary>The subjects of one country, for the quiz setup picker.</summary>
    public async Task<IActionResult> SubjectsByCountry(string country) =>
        Json((await subjects.ByCountryAsync(country)).Select(s => new { id = s.SubjectID, name = s.SubjectName }));

    public async Task<IActionResult> NewQuiz()
    {
        if (!User.IsInRole(AppRoles.User))
            return RedirectToAction("Login", "Account");

        ViewBag.Countries = await subjects.GetCountriesAsync();
        return View(await subjects.GetGeneralAsync());
    }

    public async Task<IActionResult> SaveQuiz(string title, int subjectid, int totalquestions)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("NewQuiz");

        var userId = User.IsInRole(AppRoles.User) ? User.GetUserId() : null;
        if (userId is null)
            return RedirectToAction("Login", "Account");

        // The user comes from the sign-in cookie, not from a form field anyone could edit.
        var quiz = new Quiz { Title = title, SubjectID = subjectid, TotalQuestions = totalquestions, UserID = userId.Value };
        return View(await quizzes.StartAsync(quiz));
    }

    [HttpPost]
    public async Task<IActionResult> InsertQuizQuestion([FromBody] QuizSubmission quizquestioncontract)
    {
        if (!User.IsInRole(AppRoles.User))
            return Unauthorized();

        var result = await quizzes.SubmitAsync(quizquestioncontract);
        result.Name = User.Identity?.Name;
        return Json(result);
    }

    public async Task<IActionResult> GeTQuizById(int quizID) => View(await quizzes.GetByIdAsync(quizID));
}
