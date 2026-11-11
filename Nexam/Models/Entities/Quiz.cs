namespace Nexam.Models.Entities;

public class Quiz
{
    public int QuizID { get; set; }
    public string SubjectName { get; set; }
    public int SubjectID { get; set; }
    public string Title { get; set; }
    public int UserID { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public string InsertDate { get; set; }
    public string UpdateDate { get; set; }
    public int TotalAttempts { get; set; }
    public int AllowedAttempts { get; set; }
    public int TimeLimit { get; set; }
    public List<QuizQuestion> quizQuestionDOs { get; set; }
    public List<Quiz> QuizDOs { get; set; }
}

public class QuizQuestion
{
    public int QuizID { get; set; }
    public int QuestionID { get; set; }
    public string UserAnswer { get; set; }
    public string CorrectAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int PaperID { get; set; }
    public string PaperName { get; set; }
    public string QuestionStatement { get; set; }
    public int NumberOfChoices { get; set; }
    public string Choices { get; set; }
    public string Subject { get; set; }
    public string Title { get; set; }
    public int Score { get; set; }
}
