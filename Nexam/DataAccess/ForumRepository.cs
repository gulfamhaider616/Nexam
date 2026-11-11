using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class ForumRepository : DapperRepository, IForumRepository
{
    public ForumRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<bool> SaveAsync(ForumQuestion q) =>
        ProcReturnsRowAsync("SaveForumQuestion", new { q.UserName, q.Email, q.Country, q.Subject, q.Explanation });

    public async Task<List<ForumQuestion>> GetAllAsync() =>
        (await QueryProcAsync<ForumRow>("GetAllForumQuestions")).Select(ToQuestion).ToList();

    public async Task<List<ForumQuestion>> GetPublishedAsync() =>
        (await QueryProcAsync<ForumRow>("GetAllPublishQuestions")).Select(ToQuestion).ToList();

    public async Task<List<ForumQuestion>> GetUnpublishedAsync() =>
        (await QueryProcAsync<ForumRow>("GetAllUnPublishQuestions")).Select(ToQuestion).ToList();

    public async Task<ForumQuestion> GetByIdAsync(int questionId)
    {
        var row = await QueryFirstProcAsync<ForumRow>("GetSingleQuestion", new { QuestionID = questionId });
        return row is null ? null : ToQuestion(row);
    }

    public async Task<bool> PublishAsync(int questionId) =>
        await ExecuteProcAsync("PublishQuestionByAdmin", new { QuestionID = questionId }) > 0;

    public async Task<bool> DeleteAsync(int questionId) =>
        await ExecuteProcAsync("DeleteQuestions", new { QuestionID = questionId }) > 0;

    private static ForumQuestion ToQuestion(ForumRow r) => new()
    {
        AskQuestionID = r.QuestionID,
        UserName = r.UserName,
        Email = r.Email,
        Country = r.Country,
        Subject = r.Subject,
        Explanation = r.Explanation,
        Publish = r.IsPublish ?? 0
    };
}
