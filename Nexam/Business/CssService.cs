using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class CssService(ICssRepository css) : ICssService
{
    public Task<List<Paper>> GetPapersAsync() => css.GetPapersAsync();

    /// <returns>The paper with the list of all papers, or null when the paper does not exist.</returns>
    public async Task<QuestionListViewModel> GetPaperAsync(int paperId)
    {
        var paper = await css.GetPaperByIdAsync(paperId);
        if (paper is null) return null;
        return new QuestionListViewModel
        {
            PaperDetail = paper,
            PaperList = await css.GetPapersAsync()
        };
    }

    public Task<bool> SavePaperAsync(Paper paper) => css.SavePaperAsync(paper);
}
