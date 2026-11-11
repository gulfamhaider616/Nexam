using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.Business;

public sealed class ForumService(IForumRepository forum) : IForumService
{
    public Task<bool> SaveAsync(ForumQuestion question) => forum.SaveAsync(question);

    public Task<List<ForumQuestion>> GetAllAsync() => forum.GetAllAsync();

    public Task<List<ForumQuestion>> GetPublishedAsync() => forum.GetPublishedAsync();

    public Task<List<ForumQuestion>> GetUnpublishedAsync() => forum.GetUnpublishedAsync();

    public Task<ForumQuestion> GetByIdAsync(int questionId) => forum.GetByIdAsync(questionId);

    public Task<bool> PublishAsync(int questionId) => forum.PublishAsync(questionId);

    public Task<bool> DeleteAsync(int questionId) => forum.DeleteAsync(questionId);
}
