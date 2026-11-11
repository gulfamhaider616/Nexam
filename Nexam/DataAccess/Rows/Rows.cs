namespace Nexam.DataAccess.Rows;

// Result-set shapes of the stored procedures whose column names differ from the entity property names.
// Columns that already match an entity property (Subject, Question, simple Paper...) map straight to the entity.

internal sealed class AdminRow
{
    public string AdminName { get; set; }
}

internal sealed class ContactRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public DateTime ContactDate { get; set; }
}

internal sealed class UserRow
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}

internal sealed class ForumRow
{
    public int QuestionID { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string Subject { get; set; }
    public string Explanation { get; set; }
    public int? IsPublish { get; set; }
}

internal sealed class UserQuizRow
{
    public int QuizID { get; set; }
    public string SubjectName { get; set; }
    public string Title { get; set; }
    public string Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime? InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int TotalAttempt { get; set; }
    public int AllowedAttempt { get; set; }
    public int TimeLimit { get; set; }
}

internal sealed class QuizIdRow
{
    public int QuizID { get; set; }
}

internal sealed class PpscPaperRow
{
    public int PaperID { get; set; }
    public string YearValue { get; set; }
    public string PostName { get; set; }
    public string PaperName { get; set; }
    public string PaperDetail { get; set; }
    public int? YearID { get; set; }
    public int? PostID { get; set; }
}

internal sealed class CssPaperRow
{
    public int PaperID { get; set; }
    public string PaperName { get; set; }
    public string PaperDetail { get; set; }
    public string QuestionText { get; set; }
    public string PaperSlug { get; set; }
}

internal sealed class YearRow
{
    public int YearID { get; set; }
    public string YearValue { get; set; }
}

internal sealed class PostRow
{
    public int PostID { get; set; }
    public string PostName { get; set; }
}
