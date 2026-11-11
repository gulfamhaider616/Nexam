namespace Nexam.Models.Entities;

public class Question
{
    public int QuestionID { get; set; }
    public int SubjectID { get; set; }
    public int PaperID { get; set; }
    public string SubjectName { get; set; }
    public string PaperName { get; set; }
    public string QuestionStatement { get; set; }
    public int NumberOfChoices { get; set; }
    public string Choices { get; set; }
    public string CorrectAnswer { get; set; }
    public List<QuestionOption> Options { get; set; }
}

public class QuestionOption
{
    public string Choice { get; set; }
}
