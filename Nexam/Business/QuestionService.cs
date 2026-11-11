using Nexam.Business.Helpers;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class QuestionService(IQuestionRepository questions, ISubjectRepository subjects) : IQuestionService
{
    public Task<Question> GetByIdAsync(int questionId) => questions.GetByIdAsync(questionId);

    public async Task<QuestionListViewModel> GetBySubjectAsync(int subjectId)
    {
        var list = (await questions.GetBySubjectAsync(subjectId)).WithOptions();
        return new QuestionListViewModel
        {
            questions = list,
            Subject = list.FirstOrDefault()?.SubjectName,
            SubjectList = await subjects.GetAllAsync()
        };
    }

    private const int SearchLimit = 200;

    public async Task<QuestionListViewModel> SearchAsync(string search, int? subjectId = null)
    {
        var (found, total) = string.IsNullOrWhiteSpace(search)
            ? (new List<Question>(), 0)
            : await questions.SearchAsync(search.Trim(), subjectId, SearchLimit);
        return new QuestionListViewModel
        {
            Search = search?.Trim(),
            SelectedSubjectId = subjectId,
            TotalCount = total,
            questions = found.WithOptions(),
            SubjectList = await subjects.GetAllAsync()
        };
    }

    public async Task<(List<Question> Items, int Total)> SuggestAsync(string search, int? subjectId, int limit = 6) =>
        string.IsNullOrWhiteSpace(search) ? (new List<Question>(), 0) : await questions.SearchAsync(search.Trim(), subjectId, limit);

    public Task<bool> SaveAsync(Question question) => questions.SaveAsync(question);

    public Task<bool> UpdateAsync(Question question) => questions.UpdateAsync(question);

    public Task<bool> DeleteAsync(int questionId) => questions.DeleteAsync(questionId);
}
