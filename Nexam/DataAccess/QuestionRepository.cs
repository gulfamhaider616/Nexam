using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

/// <summary>General-knowledge MCQs, grouped by subject.</summary>
public sealed class QuestionRepository : DapperRepository, IQuestionRepository
{
    public QuestionRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<Question> GetByIdAsync(int questionId) =>
        QueryFirstProcAsync<Question>("GetQuestionById", new { QuestionID = questionId });

    // Latest first: the newest past paper's questions on top (in paper order), then the question bank, newest added first.
    public async Task<List<Question>> GetBySubjectAsync(int subjectId) =>
        (await QueryProcAsync<Question>("GetAllQuestionsBySubjectID", new { SubjectID = subjectId }))
            .OrderByDescending(q => q.PaperID)
            .ThenBy(q => q.PaperID == 0 ? -q.QuestionID : q.QuestionID)
            .ToList();

    public Task<List<Question>> SearchAsync(string search) =>
        QueryProcAsync<Question>("SearchResult", new { Search = search });

    public Task<bool> SaveAsync(Question q) =>
        ProcReturnsRowAsync("SaveQuestion", new
        {
            q.SubjectID, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer
        });

    public Task<bool> UpdateAsync(Question q) =>
        ProcReturnsRowAsync("SaveEditQuestion", new
        {
            q.QuestionID, q.SubjectID, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer
        });

    public async Task<bool> DeleteAsync(int questionId) =>
        await ExecuteProcAsync("DeleteQuestionById", new { QuestionID = questionId }) > 0;
}
