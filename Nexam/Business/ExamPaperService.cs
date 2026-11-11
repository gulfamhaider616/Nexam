using Nexam.Business.Helpers;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

/// <summary>NTS and FPSC behave identically; each only differs by the repository it is given.</summary>
public abstract class ExamPaperService : IExamPaperService
{
    private readonly IExamPaperRepository _papers;

    protected ExamPaperService(IExamPaperRepository papers) => _papers = papers;

    public Task<List<Paper>> GetPapersAsync() => _papers.GetPapersAsync();

    public Task<bool> SavePaperAsync(Paper paper) => _papers.SavePaperAsync(paper);

    public async Task<QuestionListViewModel> GetPaperWithQuestionsAsync(int paperId)
    {
        var questions = (await _papers.GetQuestionsByPaperAsync(paperId)).WithOptions();
        return new QuestionListViewModel
        {
            questions = questions,
            Paper = questions.FirstOrDefault()?.PaperName,
            PaperID = questions.FirstOrDefault()?.PaperID ?? 0,
            PaperList = await _papers.GetPapersAsync()
        };
    }

    public Task<Question> GetQuestionByIdAsync(int questionId) => _papers.GetQuestionByIdAsync(questionId);

    public Task<bool> SaveQuestionAsync(Question question) => _papers.SaveQuestionAsync(question);

    public Task<bool> DeleteQuestionAsync(int questionId) => _papers.DeleteQuestionAsync(questionId);
}

public sealed class NtsService(INtsRepository papers) : ExamPaperService(papers), INtsService;

public sealed class FpscService(IFpscRepository papers) : ExamPaperService(papers), IFpscService;
