using Nexam.Models.Entities;

namespace Nexam.Models.ViewModels;

public class HomeViewModel
{
    public List<Subject> SubjectList { get; set; }
    public List<Paper> PPSCList { get; set; }
    public List<Paper> FPSCList { get; set; }
    public List<Paper> NTSList { get; set; }
    public List<Paper> CSSList { get; set; }

    /// <summary>Years that have PPSC papers, newest first, for the "Recent PPSC Papers" box.</summary>
    public List<PaperYear> RecentPpscYears { get; set; } = new();
}

public class QuestionListViewModel
{
    /// <summary>Search page: the text searched, the subject filter, and how many questions matched in total.</summary>
    public string Search { get; set; }
    public int? SelectedSubjectId { get; set; }
    public int TotalCount { get; set; }
    public List<Question> questions { get; set; }
    public string Subject { get; set; }
    public string Paper { get; set; }
    public int PaperID { get; set; }
    public List<Subject> SubjectList { get; set; }
    public List<Paper> PaperList { get; set; }
    public Paper PaperDetail { get; set; }
}

public class NewPaperViewModel
{
    public List<PaperYear> PaperYear { get; set; }
    public List<PaperPost> PaperPost { get; set; }
    public Paper Paper { get; set; }
}

/// <summary>Quiz started by the user: the new quiz id plus the questions to answer.</summary>
public class QuizSubmission
{
    public int QuizID { get; set; }
    public int UserID { get; set; }
    public List<QuizQuestion> QuizQuestions { get; set; }
    public List<Question> QuestionsDOs { get; set; }
}

public class QuizResult
{
    public string Score { get; set; }
    public string Name { get; set; }
    public string Remarks { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
}
