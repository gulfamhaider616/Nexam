using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Controllers;

public class PPSCController(IPpscService ppsc, ISubjectService subjects) : Controller
{
    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult PPSC_Home() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> NewPaper() => View(await ppsc.GetNewPaperModelAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult NewPPSCPost() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveNewPaper(int paperyear, int paperpost, string name, string detail)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("NewPaper");

        var saved = await ppsc.SavePaperAsync(new Paper
        {
            PaperYearID = paperyear, PaperPostID = paperpost, PaperName = name, PaperDetail = detail
        });
        return RedirectToAction(saved ? "PPSC_Home" : "NewPaper");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveUpdatedPPSCPaper(int paperid, int paperyear, int paperpost, string name, string detail)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("NewPaper");

        var saved = await ppsc.UpdatePaperAsync(new Paper
        {
            PaperID = paperid, PaperYearID = paperyear, PaperPostID = paperpost, PaperName = name, PaperDetail = detail
        });
        return RedirectToAction(saved ? "GetAllPPSCPapersByAdmin" : "NewPaper");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SavePPSCPost(string postname, string postdetail)
    {
        var saved = await ppsc.SavePostAsync(new JobPost { PostName = postname, PostDetail = postdetail });
        return RedirectToAction(saved ? "NewPPSCPost" : "NewPaper");
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> NewPPSCQuestion()
    {
        var papers = ppsc.GetPapersAsync();
        var subjectList = subjects.GetAllAsync();
        await Task.WhenAll(papers, subjectList);
        return View(new HomeViewModel { PPSCList = papers.Result, SubjectList = subjectList.Result });
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveNewPPSCQuestion(int paperid, int subjectid, string questionstatement,
        int numberofchoices, string choices, string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("PPSC_Home");

        var saved = await ppsc.SaveQuestionAsync(new Question
        {
            PaperID = paperid, SubjectID = subjectid, QuestionStatement = questionstatement,
            NumberOfChoices = numberofchoices, Choices = choices, CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "NewPPSCQuestion" : "PPSC_Home");
    }

    public async Task<IActionResult> GetAllPPSCPapers() => View(await ppsc.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllPPSCPapersByAdmin() => View(await ppsc.GetPapersAsync());

    public async Task<IActionResult> GetAllQuestionsByPaperID(int paperid) =>
        View(await ppsc.GetPaperWithQuestionsAsync(paperid));

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllQuestionsByPaperIDAdmin(int paperid) =>
        View(await ppsc.GetPaperWithQuestionsAsync(paperid));

    public async Task<IActionResult> GetPPSCQuestionById(int questionID)
    {
        var question = await ppsc.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeletePPSCQuestionById(int questionID)
    {
        await ppsc.DeleteQuestionAsync(questionID);
        return RedirectToAction("GetAllPPSCPapersByAdmin");
    }

    public async Task<IActionResult> PPSCQuestionDiscussion(int questionID)
    {
        var question = await ppsc.GetQuestionByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> UpdatePPSCPaper(int paperid)
    {
        var model = await ppsc.GetUpdatePaperModelAsync(paperid);
        return model.Paper is null ? NotFound() : View(model);
    }

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveEditPPSCQuestion(int questionid, int subjectid, string questionstatement,
        int numberofchoices, string choices, string correctanswer)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("PPSC_Home");

        var saved = await ppsc.UpdateQuestionAsync(new Question
        {
            QuestionID = questionid, SubjectID = subjectid, QuestionStatement = questionstatement,
            NumberOfChoices = numberofchoices, Choices = choices, CorrectAnswer = correctanswer
        });
        return RedirectToAction(saved ? "GetAllPPSCPapersByAdmin" : "PPSC_Home");
    }

    public async Task<IActionResult> Download_PPSC_Paper(int PaperID, [FromServices] ILogger<PPSCController> logger)
    {
        try
        {
            var pdf = await ppsc.GetPaperPdfAsync(PaperID);
            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");
        }
        catch (PdfUnavailableException ex)
        {
            logger.LogError(ex, "PDF download unavailable");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "PDF downloads are temporarily unavailable.");
        }
    }

    public async Task<IActionResult> SearchPPSCPaperYearwise(int yearid) =>
        View(await ppsc.SearchPapersByYearAsync(yearid));
}
