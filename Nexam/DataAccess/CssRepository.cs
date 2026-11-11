using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class CssRepository : DapperRepository, ICssRepository
{
    public CssRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<List<Paper>> GetPapersAsync() => QueryProcAsync<Paper>("GetAllCSSPaper");

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
