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

    public async Task<QuestionListViewModel> SearchAsync(string search)
    {
        var list = (await questions.SearchAsync(search)).WithOptions();
        return new QuestionListViewModel
        {
            questions = list,
            SubjectList = await subjects.GetAllAsync()
        };
    }

    public Task<bool> SaveAsync(Question question) => questions.SaveAsync(question);

    public Task<bool> UpdateAsync(Question question) => questions.UpdateAsync(question);

    public Task<bool> DeleteAsync(int questionId) => questions.DeleteAsync(questionId);
}
