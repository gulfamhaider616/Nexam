using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class CssRepository : DapperRepository, ICssRepository
{
    public CssRepository(IDbConnectionFactory factory) : base(factory) { }

    // CSS papers have no year column; the year is part of the paper name ("... Past Paper 2017"). Latest year first.
    public async Task<List<Paper>> GetPapersAsync() =>
        (await QueryProcAsync<Paper>("GetAllCSSPaper"))
            .OrderByDescending(p => YearInName(p.PaperName))
            .ThenByDescending(p => p.PaperID)
            .ToList();

    private static int YearInName(string name)
    {
        var matches = System.Text.RegularExpressions.Regex.Matches(name ?? "", @"\b(19|20)\d{2}\b");
        return matches.Count > 0 ? int.Parse(matches[^1].Value) : int.MinValue;
    }

    public async Task<Paper> GetPaperByIdAsync(int paperId)
    {
        var row = await QueryFirstProcAsync<CssPaperRow>("GetCompleteCSSPaperByID", new { PaperID = paperId });
        return row is null ? null : new Paper
        {
            PaperID = row.PaperID,
            PaperName = row.PaperName,
            PaperDetail = row.PaperDetail,
            QuestionsText = row.QuestionText,
            PaperSlug = row.PaperSlug
        };
    }

    public Task<bool> SavePaperAsync(Paper paper) =>
        ProcReturnsRowAsync("SaveCSSPaper", new
        {
            paper.PaperName,
            paper.PaperDetail,
            paper.QuestionsText,
            PaperSlug = (paper.PaperName ?? string.Empty).Replace(' ', '-')
        });
}
