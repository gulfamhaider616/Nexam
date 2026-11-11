using Nexam.Business.Helpers;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class QuestionService(IQuestionRepository questions, ISubjectService subjects) : IQuestionService
{
    public Task<Question> GetByIdAsync(int questionId) => questions.GetByIdAsync(questionId);

    public async Task<QuestionListViewModel> GetBySubjectAsync(int subjectId)
    {
        var list = (await questions.GetBySubjectAsync(subjectId)).WithOptions();
        return new QuestionListViewModel
        {
            questions = list,
            Subject = list.FirstOrDefault()?.SubjectName,
            SubjectList = await subjects.GetGeneralAsync()
        };
    }

    private const int SearchLimit = 60;

    public async Task<QuestionListViewModel> SearchAsync(string search, int? subjectId = null, string country = null)
    {
        var (found, total) = string.IsNullOrWhiteSpace(search)
            ? (new List<Question>(), 0)
            : await questions.SearchAsync(search.Trim(), subjectId, country, SearchLimit);
        return new QuestionListViewModel
        {
            Search = search?.Trim(),
            SelectedSubjectId = subjectId,
            SelectedCountry = country,
            TotalCount = total,
            questions = found.WithOptions(),
            SubjectList = await subjects.GetGeneralAsync(),
            Countries = await subjects.GetCountriesAsync()
        };
    }

    public async Task<(List<Question> Items, int Total)> SuggestAsync(string search, int? subjectId, int limit = 6, string country = null) =>
        string.IsNullOrWhiteSpace(search) ? (new List<Question>(), 0) : await questions.SearchAsync(search.Trim(), subjectId, country, limit);

    public Task<bool> SaveAsync(Question question) => questions.SaveAsync(question);

    public Task<bool> UpdateAsync(Question question) => questions.UpdateAsync(question);

    public Task<bool> DeleteAsync(int questionId) => questions.DeleteAsync(questionId);
}
