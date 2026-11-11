using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
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
        var subjectList = subjects.GetGeneralAsync();
        var countryList = subjects.GetCountriesAsync();
        var ppscList = ppsc.GetPapersAsync();
        var ntsList = nts.GetPapersAsync();
        var fpscList = fpsc.GetPapersAsync();
        var cssList = css.GetPapersAsync();
        await Task.WhenAll(subjectList, countryList, ppscList, ntsList, fpscList, cssList);

        return View(new HomeViewModel
        {
            SubjectList = subjectList.Result,
            Countries = countryList.Result,
            SubjectCount = countryList.Result.Sum(c => c.Count),
            PPSCList = ppscList.Result,
            NTSList = ntsList.Result,
            FPSCList = fpscList.Result,
            CSSList = cssList.Result,
            RecentPpscYears = LatestPpscYears(ppscList.Result, count: 5)
        });
    }

    /// <summary>The newest years that actually have papers (a paper's PaperYearID links it to the year search).</summary>
    private static List<PaperYear> LatestPpscYears(IEnumerable<Paper> papers, int count) =>
        papers
            .Where(p => p.PaperYearID > 0 && int.TryParse(p.PaperYear, out _))
            .GroupBy(p => new { p.PaperYearID, p.PaperYear })
            .OrderByDescending(g => int.Parse(g.Key.PaperYear))
            .Take(count)
            .Select(g => new PaperYear { YearID = g.Key.PaperYearID, Year = g.Key.PaperYear })
            .ToList();

    public IActionResult About() => View();

    public IActionResult Contact() => View();

    public IActionResult Terms() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();

    public async Task<IActionResult> Subjects(string country, string q, int page = 1) =>
        View(await subjects.PageAsync(country, q, page));

    public async Task<IActionResult> GetAllQuestionsBySubjectID(int SubjectID) =>
        View(await questions.GetBySubjectAsync(SubjectID));

    public async Task<IActionResult> SaveContact(string name, string email, string subject)
    {
        var saved = await contacts.SaveAsync(new ContactMessage { Name = name, Email = email, Subject = subject });
        return saved ? RedirectToAction("Index") : View("Contact");
    }

    public async Task<IActionResult> SearchResult(string search, int? subject, string country) => View(await questions.SearchAsync(search, subject, country));

    /// <summary>JSON for the search-box suggestions (top matches + total).</summary>
    public async Task<IActionResult> SearchSuggest(string search, int? subject, string country)
    {
        var (items, total) = await questions.SuggestAsync(search, subject, 6, country);
        return Json(new
        {
            total,
            items = items.Select(q => new
            {
                text = SearchSuggestions.Shorten(q.QuestionStatement),
                subject = q.SubjectName,
                url = q.PaperID == 0 ? "/Question-Detail/" + q.QuestionID : "/PPSC-Past-Paper-Question-Discussion/" + q.QuestionID
            })
        });
    }

    public async Task<IActionResult> QuestionDiscussion(int questionID)
    {
        var question = await questions.GetByIdAsync(questionID);
        return question is null ? NotFound() : View(question);
    }
}
