using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

/// <summary>General-knowledge MCQs, grouped by subject.</summary>
public sealed class QuestionRepository : DapperRepository, IQuestionRepository
{
    public QuestionRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<Question> GetByIdAsync(int questionId) =>
        QueryFirstProcAsync<Question>("GetQuestionById", new { QuestionID = questionId });

    // Latest first: the newest past paper's questions on top (in paper order), then the question bank, newest added first.
    public async Task<List<Question>> GetBySubjectAsync(int subjectId) =>
        (await QueryProcAsync<Question>("GetAllQuestionsBySubjectID", new { SubjectID = subjectId }))
            .OrderByDescending(q => q.PaperID)
            .ThenBy(q => q.PaperID == 0 ? -q.QuestionID : q.QuestionID)
            .ToList();

    // Raw SQL: the SearchResult procedure only searches the question bank; this covers the PPSC papers too.
    public async Task<(List<Question> Items, int Total)> SearchAsync(string search, int? subjectId, int limit)
    {
        const string where = "q.QuestionStatement LIKE @Pattern ESCAPE '\\' AND (@SubjectID IS NULL OR q.SubjectID = @SubjectID)";
        var countSql = $"""
            SELECT (SELECT COUNT(*) FROM Questions     q WHERE {where})
                 + (SELECT COUNT(*) FROM PPSCQuestions q WHERE {where})
            """;
        var pageSql = $"""
            SELECT TOP (@Limit) * FROM (
                SELECT q.QuestionID, q.SubjectID, s.SubjectName, q.QuestionStatement, q.NumberOfChoices,
                       q.Choices, q.CorrectAnswer, CAST(0 AS int) AS PaperID
                FROM Questions q JOIN Subject s ON s.SubjectID = q.SubjectID
                WHERE {where}
                UNION ALL
                SELECT q.QuestionID, q.SubjectID, s.SubjectName, q.QuestionStatement, q.NumberOfChoices,
                       q.Choices, q.CorrectAnswer, ISNULL(q.PaperID, 0)
                FROM PPSCQuestions q JOIN Subject s ON s.SubjectID = q.SubjectID
                WHERE {where}) r
            ORDER BY PaperID DESC, QuestionID
            """;

        // The visitor's text is data, not a LIKE pattern: escape the wildcard characters.
        var escaped = (search ?? "").Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");
        var args = new { Pattern = "%" + escaped + "%", SubjectID = subjectId, Limit = limit };

        using var connection = Factory.Create();
        var total = await Dapper.SqlMapper.ExecuteScalarAsync<int>(connection, countSql, args);
        var rows = (await Dapper.SqlMapper.QueryAsync<Question>(connection, pageSql, args)).ToList();
        return (rows, total);
    }

    public Task<bool> SaveAsync(Question q) =>
        ProcReturnsRowAsync("SaveQuestion", new
        {
            q.SubjectID, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer
        });

    public Task<bool> UpdateAsync(Question q) =>
        ProcReturnsRowAsync("SaveEditQuestion", new
        {
            q.QuestionID, q.SubjectID, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer
        });

    public async Task<bool> DeleteAsync(int questionId) =>
        await ExecuteProcAsync("DeleteQuestionById", new { QuestionID = questionId }) > 0;
}
