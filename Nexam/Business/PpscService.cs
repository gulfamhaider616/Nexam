using Nexam.Business.Helpers;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class PpscService(IPpscRepository ppsc, IPdfGenerator pdf) : IPpscService
{
    public Task<List<Paper>> GetPapersAsync() => ppsc.GetPapersAsync();

    public Task<List<Paper>> SearchPapersByYearAsync(int yearId) => ppsc.SearchPapersByYearAsync(yearId);

    public Task<NewPaperViewModel> GetNewPaperModelAsync() => ppsc.GetYearsAndPostsAsync();

    public async Task<NewPaperViewModel> GetUpdatePaperModelAsync(int paperId)
    {
        var model = await ppsc.GetYearsAndPostsAsync();
        model.Paper = await ppsc.GetPaperDetailAsync(paperId);
        return model;
    }

    public Task<bool> SavePaperAsync(Paper paper) => ppsc.SavePaperAsync(paper);

    public Task<bool> UpdatePaperAsync(Paper paper) => ppsc.UpdatePaperAsync(paper);

    public Task<bool> SavePostAsync(JobPost post) => ppsc.SavePostAsync(post);

    public async Task<QuestionListViewModel> GetPaperWithQuestionsAsync(int paperId)
    {
        var questions = (await ppsc.GetQuestionsByPaperAsync(paperId)).WithOptions();
        return new QuestionListViewModel
        {
            questions = questions,
            Paper = questions.FirstOrDefault()?.PaperName,
            PaperID = questions.FirstOrDefault()?.PaperID ?? 0,
            PaperList = await ppsc.GetPapersAsync()
        };
    }

    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        var question = await ppsc.GetQuestionByIdAsync(questionId);
        // Older questions have no subject; they have always been shown under subject 1.
        if (question is { SubjectID: 0 }) question.SubjectID = 1;
        return question;
    }

    public Task<bool> SaveQuestionAsync(Question question) => ppsc.SaveQuestionAsync(question);

    public Task<bool> UpdateQuestionAsync(Question question) => ppsc.UpdateQuestionAsync(question);

    public Task<bool> DeleteQuestionAsync(int questionId) => ppsc.DeleteQuestionAsync(questionId);

    public async Task<byte[]> GetPaperPdfAsync(int paperId)
    {
        var questions = await ppsc.GetQuestionsByPaperAsync(paperId);
        return await pdf.FromHtmlAsync(PaperHtmlBuilder.Build(questions));
    }
}
