using Dapper;
using System.Data;
using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.DataAccess;

public sealed class PpscRepository : DapperRepository, IPpscRepository
{
    public PpscRepository(IDbConnectionFactory factory) : base(factory) { }

    // ---------- papers ----------

    public async Task<List<Paper>> GetPapersAsync() =>
        (await QueryProcAsync<PpscPaperRow>("GetAllPaper")).Select(ToPaper).ToList();

    public async Task<List<Paper>> SearchPapersByYearAsync(int yearId) =>
        (await QueryProcAsync<PpscPaperRow>("SearchPPSCPaperYearwise", new { YearID = yearId })).Select(ToPaper).ToList();

    // Raw SQL: the GetPPSCPaperDetail procedure does not exist in the database.
    public async Task<Paper> GetPaperDetailAsync(int paperId)
    {
        const string sql = """
            SELECT P.PaperID, Y.YearValue, J.PostName, P.PaperName, P.PaperDetail, P.YearID, P.PostID
            FROM PPSCPaper P
            LEFT JOIN PaperYear Y ON Y.YearID = P.YearID
            LEFT JOIN JobPost   J ON J.PostID = P.PostID
            WHERE P.PaperID = @PaperID
            """;
        var row = await QueryFirstSqlAsync<PpscPaperRow>(sql, new { PaperID = paperId });
        return row is null ? null : ToPaper(row);
    }

    public Task<bool> SavePaperAsync(Paper paper) =>
        ProcReturnsRowAsync("SavePPSCPaper", new
        {
            PaperYearID = paper.PaperYearID, PaperPostID = paper.PaperPostID, paper.PaperName, paper.PaperDetail
        });

    // Raw SQL: the UpdatePPSCPaper procedure does not exist in the database.
    public async Task<bool> UpdatePaperAsync(Paper paper)
    {
        const string sql = """
            UPDATE PPSCPaper
            SET PaperName = @PaperName, PaperDetail = @PaperDetail, YearID = @PaperYearID, PostID = @PaperPostID
            WHERE PaperID = @PaperID
            """;
        return await ExecuteSqlAsync(sql, new
        {
            paper.PaperID, paper.PaperName, paper.PaperDetail, paper.PaperYearID, paper.PaperPostID
        }) > 0;
    }

    // Raw SQL: the SavePPSCPost procedure does not exist in the database.
    public async Task<bool> SavePostAsync(JobPost post)
    {
        const string sql = "INSERT INTO JobPost (PostName, PostDetail) VALUES (@PostName, @PostDetail)";
        return await ExecuteSqlAsync(sql, new { post.PostName, post.PostDetail }) > 0;
    }

    public async Task<NewPaperViewModel> GetYearsAndPostsAsync()
    {
        using var connection = Factory.Create();
        // GetPaperYearPost returns two result sets: years, then job posts.
        using var grid = await connection.QueryMultipleAsync("GetPaperYearPost", commandType: CommandType.StoredProcedure);
        var years = (await grid.ReadAsync<YearRow>()).Select(y => new PaperYear { YearID = y.YearID, Year = y.YearValue }).ToList();
        var posts = (await grid.ReadAsync<PostRow>()).Select(p => new PaperPost { PostID = p.PostID, Post = p.PostName }).ToList();
        return new NewPaperViewModel { PaperYear = years, PaperPost = posts };
    }

    // ---------- questions ----------

    public Task<List<Question>> GetQuestionsByPaperAsync(int paperId) =>
        QueryProcAsync<Question>("GetAllQuestionsByPaperID", new { PaperID = paperId });

    public Task<Question> GetQuestionByIdAsync(int questionId) =>
        QueryFirstProcAsync<Question>("GetPPSCQuestionById", new { QuestionID = questionId });

    public Task<bool> SaveQuestionAsync(Question q) =>
        ProcReturnsRowAsync("SavePPSCQuestion", new
        {
            q.PaperID, q.SubjectID, q.QuestionStatement, q.Choices, q.CorrectAnswer, q.NumberOfChoices
        });

    public Task<bool> UpdateQuestionAsync(Question q) =>
        ProcReturnsRowAsync("SaveEditPPSCQuestion", new
        {
            q.QuestionID, q.SubjectID, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer
        });

    public async Task<bool> DeleteQuestionAsync(int questionId) =>
        await ExecuteProcAsync("DeletePPSCQuestionById", new { QuestionID = questionId }) > 0;

    private static Paper ToPaper(PpscPaperRow r) => new()
    {
        PaperID = r.PaperID,
        PaperYear = r.YearValue,
        PaperPost = r.PostName,
        PaperName = r.PaperName,
        PaperDetail = r.PaperDetail,
        PaperYearID = r.YearID ?? 0,
        PaperPostID = r.PostID ?? 0
    };
}
