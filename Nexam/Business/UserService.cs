using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.Business;

public sealed class UserService(IUserRepository users) : IUserService
{
    public Task<bool> RegisterAsync(UserAccount user) => users.SaveAsync(user);

    public Task<UserAccount> VerifyAsync(string email, string password) => users.VerifyAsync(email, password);

    public Task<List<UserAccount>> GetAllAsync() => users.GetAllAsync();

    public Task<UserAccount> GetProfileAsync(int userId) => users.GetProfileAsync(userId);

    public Task<List<Quiz>> GetQuizzesAsync(int userId) => users.GetQuizzesAsync(userId);
}
