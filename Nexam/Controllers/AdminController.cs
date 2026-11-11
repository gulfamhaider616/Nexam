using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class AdminController(ISubjectService subjects, IQuestionService questions,
    IContactService contacts) : Controller
{
    // The old separate admin login now lives at /Login (kept so bookmarks still work).
    public IActionResult Login() => RedirectToActionPermanent("Login", "Account");

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllSubjects(string country, string q, int page = 1) =>
        View(await subjects.PageAsync(Request.Query.ContainsKey("country") ? country : "General", q, page));

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult AddNewSubject() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> AddNewQuestion() => View(await subjects.GetGeneralAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetContacts() => View(await contacts.GetAllAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveSubject(string subjectname, string subjectdetail)
    {
        var saved = await subjects.SaveAsync(new Subject { SubjectName = subjectname, SubjectDetail = subjectdetail });
        return RedirectToAction(saved ? "GetAllSubjects" : "AddNewSubject");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveQuestion(int subjectid, string questionstatement, int numberofchoices,
        string choices, string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("GetAllSubjects");

        var saved = await questions.SaveAsync(new Question
        {
            SubjectID = subjectid,
            QuestionStatement = questionstatement,
            NumberOfChoices = numberofchoices,
            Choices = choices,
            CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "AddNewQuestion" : "GetAllSubjects");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllQuestionsByAdmin(int SubjectID) =>
        View(await questions.GetBySubjectAsync(SubjectID));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetQuestionById(int questionID)
    {
        var question = await questions.GetByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveEditQuestion(int questionid, int subjectid, string questionstatement,
        int numberofchoices, string choices, string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("AddNewQuestion");

        var saved = await questions.UpdateAsync(new Question
        {
            QuestionID = questionid,
            SubjectID = subjectid,
            QuestionStatement = questionstatement,
            NumberOfChoices = numberofchoices,
            Choices = choices,
            CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "GetAllSubjects" : "AddNewQuestion");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> AdminSearchResult(string search) => View(await questions.SearchAsync(search));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SearchSuggest(string search)
    {
        var (items, total) = await questions.SuggestAsync(search, null);
        return Json(new
        {
            total,
            items = items.Select(q => new
            {
                text = SearchSuggestions.Shorten(q.QuestionStatement),
                subject = q.SubjectName,
                url = "/Admin/GetQuestionById?questionID=" + q.QuestionID
            })
        });
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteQuestion(int questionID)
    {
        await questions.DeleteAsync(questionID);
        return RedirectToAction("GetAllSubjects");
    }
}
