using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class FPSCController(IFpscService fpsc) : Controller
{
    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult FPSC_Home() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult NewFPSCPaper() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveFPSCNewPaper(string name, string detail)
    {
        var saved = await fpsc.SavePaperAsync(new Paper { PaperName = name, PaperDetail = detail });
        return RedirectToAction(saved ? "FPSC_Home" : "NewFPSCPaper");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> NewFPSCQuestion() => View(await fpsc.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveNewFPSCQuestion(int paperid, string questionstatement, string choices,
        string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("FPSC_Home");

        var saved = await fpsc.SaveQuestionAsync(new Question
        {
            PaperID = paperid, QuestionStatement = questionstatement, Choices = choices, CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "NewFPSCQuestion" : "FPSC_Home");
    }

    public async Task<IActionResult> GetAllFPSCPapers() => View(await fpsc.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllFPSCPapersByAdmin() => View(await fpsc.GetPapersAsync());

    public async Task<IActionResult> GetAllFPSCQuestionsByPaperID(int paperid) =>
        View(await fpsc.GetPaperWithQuestionsAsync(paperid));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllFPSCQuestionsByPaperIDAdmin(int paperid) =>
        View(await fpsc.GetPaperWithQuestionsAsync(paperid));

    public async Task<IActionResult> GetFPSCQuestionById(int questionID)
    {
        var question = await fpsc.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteFPSCQuestionById(int questionID)
    {
        await fpsc.DeleteQuestionAsync(questionID);
        return RedirectToAction("GetAllFPSCPapersByAdmin");
    }

    public async Task<IActionResult> FPSCQuestionDiscussion(int questionID)
    {
        var question = await fpsc.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }
}
