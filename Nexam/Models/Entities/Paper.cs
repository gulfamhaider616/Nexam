namespace Nexam.Models.Entities;

/// <summary>A past paper (CSS / PPSC / NTS / FPSC).</summary>
public class Paper
{
    public int PaperID { get; set; }
    public string PaperName { get; set; }
    public string PaperYear { get; set; }
    public int PaperYearID { get; set; }
    public int PaperPostID { get; set; }
    public string PaperPost { get; set; }
    public string PaperDetail { get; set; }
    public string QuestionsText { get; set; }
    public string PaperSlug { get; set; }
}

public class PaperYear
{
    public string Year { get; set; }
    public int YearID { get; set; }
}

public class PaperPost
{
    public string Post { get; set; }
    public int PostID { get; set; }
}

/// <summary>A job post a PPSC paper belongs to.</summary>
public class JobPost
{
    public int PostID { get; set; }
    public string PostName { get; set; }
    public string PostDetail { get; set; }
}
