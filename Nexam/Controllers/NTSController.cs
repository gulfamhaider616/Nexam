using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class NTSController(INtsService nts) : Controller
{
    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult NTS_Home() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult NewNTSPaper() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveNTSNewPaper(string name, string detail)
    {
        var saved = await nts.SavePaperAsync(new Paper { PaperName = name, PaperDetail = detail });
        return RedirectToAction(saved ? "NTS_Home" : "NewNTSPaper");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> NewNTSQuestion() => View(await nts.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveNewNTSQuestion(int paperid, string questionstatement, string choices,
        string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("NTS_Home");

        var saved = await nts.SaveQuestionAsync(new Question
        {
            PaperID = paperid, QuestionStatement = questionstatement, Choices = choices, CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "NewNTSQuestion" : "NTS_Home");
    }

    public async Task<IActionResult> GetAllNTSPapers() => View(await nts.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllNTSPapersByAdmin() => View(await nts.GetPapersAsync());

    public async Task<IActionResult> GetAllNTSQuestionsByPaperID(int paperid) =>
        View(await nts.GetPaperWithQuestionsAsync(paperid));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllNTSQuestionsByPaperIDAdmin(int paperid) =>
        View(await nts.GetPaperWithQuestionsAsync(paperid));

    public async Task<IActionResult> GetNTSQuestionById(int questionID)
    {
        var question = await nts.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteNTSQuestionById(int questionID)
    {
        await nts.DeleteQuestionAsync(questionID);
        return RedirectToAction("GetAllNTSPapersByAdmin");
    }

    public async Task<IActionResult> NTSQuestionDiscussion(int questionID)
    {
        var question = await nts.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }
}
