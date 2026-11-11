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
    // Built for a large bank (a million+ rows): every query stops early (TOP), results come newest-first from the
    // clustered key so no big sort is needed, and the total is only counted up to CountCap.
    private const int CountCap = 1001;

    public async Task<(List<Question> Items, int Total)> SearchAsync(string search, int? subjectId, string country, int limit)
    {
        // The visitor's text is data, not a LIKE pattern: escape the wildcard characters.
        string Esc(string s) => (s ?? "").Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");
        var general = string.IsNullOrWhiteSpace(country) || country == "General";   // PPSC papers belong to the general subjects
        var countryFilter = string.IsNullOrWhiteSpace(country) ? ""
            : country == "General" ? " AND q.SubjectID IN (SELECT SubjectID FROM Subject WHERE SubjectName NOT LIKE '% - %')"
            : " AND q.SubjectID IN (SELECT SubjectID FROM Subject WHERE SubjectName LIKE @Prefix ESCAPE '\\')";
        var where = "q.QuestionStatement LIKE @Pattern ESCAPE '\\' AND (@SubjectID IS NULL OR q.SubjectID = @SubjectID)" + countryFilter;
        var args = new { Pattern = "%" + Esc(search) + "%", SubjectID = subjectId, Prefix = Esc(country) + " - %", Limit = limit, Cap = CountCap };

        const string columns = "q.QuestionID, q.SubjectID, s.SubjectName, q.QuestionStatement, q.NumberOfChoices, q.Choices, q.CorrectAnswer";
        async Task<List<T>> Rows<T>(string sql) { using var c = Factory.Create(); return (await Dapper.SqlMapper.QueryAsync<T>(c, sql, args)).ToList(); }

        var ppscRows = general
            ? Rows<Question>($"SELECT TOP (@Limit) {columns}, ISNULL(q.PaperID, 0) AS PaperID FROM PPSCQuestions q JOIN Subject s ON s.SubjectID = q.SubjectID WHERE {where} ORDER BY q.PaperID DESC, q.QuestionID")
            : Task.FromResult(new List<Question>());
        var ppscCount = general
            ? Rows<int>($"SELECT COUNT(*) FROM (SELECT TOP (@Cap) 1 AS x FROM PPSCQuestions q WHERE {where}) c")
            : Task.FromResult(new List<int> { 0 });
        var bankRows = Rows<Question>($"SELECT TOP (@Limit) {columns}, CAST(0 AS int) AS PaperID FROM Questions q JOIN Subject s ON s.SubjectID = q.SubjectID WHERE {where} ORDER BY q.QuestionID DESC");
        var bankCount = Rows<int>($"SELECT COUNT(*) FROM (SELECT TOP (@Cap) 1 AS x FROM Questions q WHERE {where}) c");
        await Task.WhenAll(ppscRows, ppscCount, bankRows, bankCount);

        var items = ppscRows.Result.Concat(bankRows.Result).Take(limit).ToList();
        return (items, Math.Min(ppscCount.Result[0] + bankCount.Result[0], CountCap));
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
