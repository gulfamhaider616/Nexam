using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Controllers;

public class HomeController(
    ISubjectService subjects,
    IQuestionService questions,
    IContactService contacts,
    IPpscService ppsc,
    INtsService nts,
    IFpscService fpsc,
    ICssService css) : Controller
{
    public async Task<IActionResult> Index()
    {
        var subjectList = subjects.GetAllAsync();
        var ppscList = ppsc.GetPapersAsync();
        var ntsList = nts.GetPapersAsync();
        var fpscList = fpsc.GetPapersAsync();
        var cssList = css.GetPapersAsync();
        await Task.WhenAll(subjectList, ppscList, ntsList, fpscList, cssList);

        return View(new HomeViewModel
        {
            SubjectList = subjectList.Result,
            PPSCList = ppscList.Result,
            NTSList = ntsList.Result,
            FPSCList = fpscList.Result,
            CSSList = cssList.Result
        });
    }

    public IActionResult About() => View();

    public IActionResult Contact() => View();

    public IActionResult Terms() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();

    public async Task<IActionResult> Subjects() => View(await subjects.GetAllAsync());

    public async Task<IActionResult> GetAllQuestionsBySubjectID(int SubjectID) =>
        View(await questions.GetBySubjectAsync(SubjectID));

    public async Task<IActionResult> SaveContact(string name, string email, string subject)
    {
        var saved = await contacts.SaveAsync(new ContactMessage { Name = name, Email = email, Subject = subject });
        return saved ? RedirectToAction("Index") : View("Contact");
    }

    public async Task<IActionResult> SearchResult(string search) => View(await questions.SearchAsync(search));

    public async Task<IActionResult> QuestionDiscussion(int questionID)
    {
        var question = await questions.GetByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }
}
