using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business.Interfaces;

public interface IAdminService
{
    /// <returns>The admin's display name when the credentials are valid, otherwise null.</returns>
    Task<string> VerifyAsync(string email, string password);
}

public interface ISubjectService
{
    Task<List<Subject>> GetAllAsync();
    Task<bool> SaveAsync(Subject subject);
}

public interface IQuestionService
{
    /// <returns>The question, or null when it does not exist.</returns>
    Task<Question> GetByIdAsync(int questionId);
    Task<QuestionListViewModel> GetBySubjectAsync(int subjectId);
    Task<QuestionListViewModel> SearchAsync(string search, int? subjectId = null);
    /// <summary>A few matches for type-ahead suggestions, plus the total number of matches.</summary>
    Task<(List<Question> Items, int Total)> SuggestAsync(string search, int? subjectId, int limit = 6);
    Task<bool> SaveAsync(Question question);
    Task<bool> UpdateAsync(Question question);
    Task<bool> DeleteAsync(int questionId);
}

public interface IContactService
{
    Task<List<ContactMessage>> GetAllAsync();
    Task<bool> SaveAsync(ContactMessage contact);
}

public interface IUserService
{
    Task<bool> RegisterAsync(UserAccount user);
    /// <returns>The user, or null when the email/password is wrong.</returns>
    Task<UserAccount> VerifyAsync(string email, string password);
    Task<List<UserAccount>> GetAllAsync();
    Task<UserAccount> GetProfileAsync(int userId);
    Task<List<Quiz>> GetQuizzesAsync(int userId);
}

public interface IForumService
{
    Task<bool> SaveAsync(ForumQuestion question);
    Task<List<ForumQuestion>> GetAllAsync();
    Task<List<ForumQuestion>> GetPublishedAsync();
    Task<List<ForumQuestion>> GetUnpublishedAsync();
    Task<ForumQuestion> GetByIdAsync(int questionId);
    Task<bool> PublishAsync(int questionId);
    Task<bool> DeleteAsync(int questionId);
}

public interface IQuizService
{
    Task<QuizSubmission> StartAsync(Quiz quiz);
    Task<QuizResult> SubmitAsync(QuizSubmission submission);
    Task<Quiz> GetByIdAsync(int quizId);
}

/// <summary>Past papers for an exam body with the basic paper/question shape (NTS, FPSC).</summary>
public interface IExamPaperService
{
    Task<List<Paper>> GetPapersAsync();
    Task<bool> SavePaperAsync(Paper paper);
    Task<QuestionListViewModel> GetPaperWithQuestionsAsync(int paperId);
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<bool> SaveQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(int questionId);
}

public interface INtsService : IExamPaperService { }

public interface IFpscService : IExamPaperService { }

public interface IPpscService
{
    Task<List<Paper>> GetPapersAsync();
    Task<List<Paper>> SearchPapersByYearAsync(int yearId);
    Task<NewPaperViewModel> GetNewPaperModelAsync();
    Task<NewPaperViewModel> GetUpdatePaperModelAsync(int paperId);
    Task<bool> SavePaperAsync(Paper paper);
    Task<bool> UpdatePaperAsync(Paper paper);
    Task<bool> SavePostAsync(JobPost post);
    Task<QuestionListViewModel> GetPaperWithQuestionsAsync(int paperId);
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<bool> SaveQuestionAsync(Question question);
    Task<bool> UpdateQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(int questionId);
    /// <summary>Renders the paper as a downloadable PDF.</summary>
    Task<byte[]> GetPaperPdfAsync(int paperId);
}

public interface ICssService
{
    Task<List<Paper>> GetPapersAsync();
    Task<QuestionListViewModel> GetPaperAsync(int paperId);
    Task<bool> SavePaperAsync(Paper paper);
}

public interface IAnnouncementService
{
    /// <summary>Emails the registration announcement to every registered user (BCC).</summary>
    Task SendRegistrationAnnouncementAsync();
}

/// <summary>Who signed in and in which role.</summary>
public sealed record LoginResult(string Role, string Name, int? UserId);

public interface IAccountService
{
    /// <summary>Checks the credentials against admins first, then site users.</summary>
    /// <returns>The signed-in account, or null when the email/password is wrong.</returns>
    Task<LoginResult> LoginAsync(string email, string password);
}
