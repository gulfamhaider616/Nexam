using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class ForumController(IForumService forum) : Controller
{
    public async Task<IActionResult> ForumHomePage() => View(await forum.GetPublishedAsync());

    public IActionResult AskQuestion() => View();

    public async Task<IActionResult> SaveQuestion(string uname, string qemail, string qcountry, string qsubject,
        string qexplanation)
    {
        var saved = await forum.SaveAsync(new ForumQuestion
        {
            UserName = uname, Email = qemail, Country = qcountry, Subject = qsubject, Explanation = qexplanation
        });

        // The Ask-a-question page submits through AJAX and expects a boolean; plain form posts get redirected.
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Json(saved);

        return RedirectToAction("ForumHomePage", "Forum");
    }

    public async Task<IActionResult> SingleQuestion(int QuestionID)
    {
        var question = await forum.GetByIdAsync(QuestionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> ForumMainPage() => View(await forum.GetAllAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Publish() => View(await forum.GetPublishedAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> UnPublish() => View(await forum.GetUnpublishedAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> PublishQuestionByAdmin(int QuestionID) => Json(await forum.PublishAsync(QuestionID));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteQuestions(int QuestionID) => Json(await forum.DeleteAsync(QuestionID));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> AdminQuestionPreview(int QuestionID)
    {
        var question = await forum.GetByIdAsync(QuestionID);
        return question is null ? NotFound() : View(question);
    }
}
