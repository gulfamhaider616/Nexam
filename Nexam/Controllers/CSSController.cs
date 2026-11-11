using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class CSSController(ICssService css) : Controller
{
    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult CSS_Home() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult NewCSSPaper() => View();

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SaveCSSNewPaper(string name, string detail, string questionstatement)
    {
        var saved = await css.SavePaperAsync(new Paper
        {
            PaperName = name, PaperDetail = detail, QuestionsText = questionstatement
        });
        return RedirectToAction(saved ? "CSS_Home" : "NewCSSPaper");
    }

    public async Task<IActionResult> GetAllCSSPapers() => View(await css.GetPapersAsync());

    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAllCSSPapersByAdmin() => View(await css.GetPapersAsync());

    public async Task<IActionResult> GetCompleteCSSPaperByID(int paperid)
    {
        var paper = await css.GetPaperAsync(paperid);
        return paper is null ? NotFound() : View(paper);
    }
}
