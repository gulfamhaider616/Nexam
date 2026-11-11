using Nexam.Models.Entities;

namespace Nexam.Models.ViewModels;

/// <summary>A country group ("UAE", "General", ...) and how many subjects it holds.</summary>
public class CountryCount
{
    public string Country { get; set; }
    public int Count { get; set; }
}

/// <summary>One page of the Subjects list, filtered by country and/or text.</summary>
public class SubjectsPageViewModel
{
    public List<Subject> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Country { get; set; }
    public string Text { get; set; }
    public List<CountryCount> Countries { get; set; } = new();
    public int TotalSubjects { get; set; }
    public int Pages => Math.Max(1, (int)Math.Ceiling(Total / (double)Math.Max(PageSize, 1)));
}

public class HomeViewModel
{
    public List<Subject> SubjectList { get; set; }
    public List<CountryCount> Countries { get; set; }
    /// <summary>All subjects including the per-country ones (SubjectList holds only the general ones).</summary>
    public int SubjectCount { get; set; }
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
    /// <summary>Search page: the country filter, and the countries to offer in the dropdown.</summary>
    public string SelectedCountry { get; set; }
    public List<CountryCount> Countries { get; set; }
    /// <summary>Matches found (counting stops at 1001, shown as "1000+").</summary>
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
