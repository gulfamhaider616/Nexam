using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class UserRepository : DapperRepository, IUserRepository
{
    public UserRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<bool> SaveAsync(UserAccount user) =>
        ProcReturnsRowAsync("SaveUser", new
        {
            FName = user.FName, LName = user.LName, user.Email, user.Password,
            PNumber = user.PNumber, user.Country, user.City
        });

    public async Task<UserAccount> VerifyAsync(string email, string password)
    {
        var row = await QueryFirstProcAsync<UserRow>("VerifyUser", new { Email = email, Password = password });
        return row is null ? null : ToUser(row);
    }

    public async Task<List<UserAccount>> GetAllAsync() =>
        (await QueryProcAsync<UserRow>("GetAllUsers")).Select(ToUser).ToList();

    public async Task<UserAccount> GetProfileAsync(int userId)
    {
        var row = await QueryFirstProcAsync<UserRow>("GetUserProfile", new { Id = userId });
        return row is null ? null : ToUser(row);
    }

    public async Task<List<Quiz>> GetQuizzesAsync(int userId)
    {
        var rows = await QueryProcAsync<UserQuizRow>("GETUSERQUIZ", new { USERID = userId });
        return rows.Select(r => new Quiz
        {
            QuizID = r.QuizID,
            SubjectName = r.SubjectName,
            Title = r.Title,
            Score = int.TryParse(r.Score, out var score) ? score : 0,
            TotalQuestions = r.TotalQuestions,
            InsertDate = r.InsertDate?.ToString(),
            UpdateDate = r.UpdateDate?.ToString(),
            TotalAttempts = r.TotalAttempt,
            AllowedAttempts = r.AllowedAttempt,
            TimeLimit = r.TimeLimit
        }).ToList();
    }

    private static UserAccount ToUser(UserRow r) => new()
    {
        Id = r.Id,
        FName = r.FirstName,
        LName = r.LastName,
        Email = r.Email,
        Password = r.Password,
        PNumber = r.PhoneNumber,
        Country = r.Country,
        City = r.City
    };
}
