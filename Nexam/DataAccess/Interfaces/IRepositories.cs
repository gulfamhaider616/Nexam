using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.DataAccess.Interfaces;

public interface IAdminRepository
{
    /// <returns>The admin's name when the credentials are valid, otherwise null.</returns>
    Task<string> VerifyAdminAsync(string email, string password);
}

public interface ISubjectRepository
{
    Task<List<Subject>> GetAllAsync();
    Task<bool> SaveAsync(Subject subject);
}

public interface IQuestionRepository
{
    Task<Question> GetByIdAsync(int questionId);
    Task<List<Question>> GetBySubjectAsync(int subjectId);
    /// <summary>Searches the question bank and the PPSC questions; returns at most <paramref name="limit"/> rows plus the total match count.</summary>
    Task<(List<Question> Items, int Total)> SearchAsync(string search, int? subjectId, int limit);
    Task<bool> SaveAsync(Question question);
    Task<bool> UpdateAsync(Question question);
    Task<bool> DeleteAsync(int questionId);
}

public interface IContactRepository
{
    Task<List<ContactMessage>> GetAllAsync();
    Task<bool> SaveAsync(ContactMessage contact);
}

public interface IUserRepository
{
    Task<bool> SaveAsync(UserAccount user);
    /// <returns>The matching user, or null when the credentials are wrong.</returns>
    Task<UserAccount> VerifyAsync(string email, string password);
    Task<List<UserAccount>> GetAllAsync();
    Task<UserAccount> GetProfileAsync(int userId);
    Task<List<Quiz>> GetQuizzesAsync(int userId);
}

public interface IForumRepository
{
    Task<bool> SaveAsync(ForumQuestion question);
    Task<List<ForumQuestion>> GetAllAsync();
    Task<List<ForumQuestion>> GetPublishedAsync();
    Task<List<ForumQuestion>> GetUnpublishedAsync();
    Task<ForumQuestion> GetByIdAsync(int questionId);
    Task<bool> PublishAsync(int questionId);
    Task<bool> DeleteAsync(int questionId);
}

public interface IQuizRepository
{
    /// <summary>Creates the quiz and returns its id plus the questions to ask.</summary>
    Task<(int QuizId, List<Question> Questions)> CreateAsync(Quiz quiz);
    Task SaveQuestionResultAsync(QuizQuestion question);
    Task UpdateScoreAsync(int quizId, int score);
    Task<List<QuizQuestion>> GetQuestionsAsync(int quizId);
}

/// <summary>Papers + questions for an exam body that only has the basic paper/question shape (NTS, FPSC).</summary>
public interface IExamPaperRepository
{
    Task<List<Paper>> GetPapersAsync();
    Task<bool> SavePaperAsync(Paper paper);
    Task<List<Question>> GetQuestionsByPaperAsync(int paperId);
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<bool> SaveQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(int questionId);
}

public interface INtsRepository : IExamPaperRepository { }

public interface IFpscRepository : IExamPaperRepository { }

public interface IPpscRepository
{
    Task<List<Paper>> GetPapersAsync();
    Task<List<Paper>> SearchPapersByYearAsync(int yearId);
    Task<Paper> GetPaperDetailAsync(int paperId);
    Task<bool> SavePaperAsync(Paper paper);
    Task<bool> UpdatePaperAsync(Paper paper);
    Task<bool> SavePostAsync(JobPost post);
    Task<NewPaperViewModel> GetYearsAndPostsAsync();
    Task<List<Question>> GetQuestionsByPaperAsync(int paperId);
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<bool> SaveQuestionAsync(Question question);
    Task<bool> UpdateQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(int questionId);
}

public interface ICssRepository
{
    Task<List<Paper>> GetPapersAsync();
    Task<Paper> GetPaperByIdAsync(int paperId);
    Task<bool> SavePaperAsync(Paper paper);
}
