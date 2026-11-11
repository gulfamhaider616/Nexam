using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.DataAccess;

public sealed class SubjectRepository : DapperRepository, ISubjectRepository
{
    public SubjectRepository(IDbConnectionFactory factory) : base(factory) { }

    /// <summary>Subjects named "COUNTRY - EXAM" belong to a country; every other subject is "General".</summary>
    private const string IsCountrySubject = "SubjectName LIKE '% - %'";

    public Task<List<Subject>> GetAllAsync() => QueryProcAsync<Subject>("GetAllSubjects");

    public Task<List<Subject>> GetGeneralAsync() =>
        QuerySqlAsync<Subject>($"SELECT * FROM Subject WHERE NOT ({IsCountrySubject}) ORDER BY SubjectID");

    public Task<List<CountryCount>> GetCountriesAsync() =>
        QuerySqlAsync<CountryCount>($"""
            SELECT CASE WHEN CHARINDEX(' - ', SubjectName) > 0 THEN LEFT(SubjectName, CHARINDEX(' - ', SubjectName) - 1) ELSE 'General' END AS Country,
                   COUNT(*) AS Count
            FROM Subject
            GROUP BY CASE WHEN CHARINDEX(' - ', SubjectName) > 0 THEN LEFT(SubjectName, CHARINDEX(' - ', SubjectName) - 1) ELSE 'General' END
            """);

    public async Task<(List<Subject> Items, int Total)> PageAsync(string country, string text, int page, int pageSize)
    {
        var where = new List<string> { "1 = 1" };
        if (!string.IsNullOrWhiteSpace(country))
            where.Add(country == "General" ? $"NOT ({IsCountrySubject})" : "SubjectName LIKE @Prefix ESCAPE '\\'");
        if (!string.IsNullOrWhiteSpace(text))
            where.Add("SubjectName LIKE @Text ESCAPE '\\'");
        var condition = string.Join(" AND ", where);

        var args = new
        {
            Prefix = Like(country) + " - %",
            Text = "%" + Like(text) + "%",
            Skip = (Math.Max(page, 1) - 1) * pageSize,
            Take = pageSize
        };

        var total = (await QuerySqlAsync<int>($"SELECT COUNT(*) FROM Subject WHERE {condition}", args)).First();
        var items = await QuerySqlAsync<Subject>($"""
            SELECT * FROM Subject WHERE {condition}
            ORDER BY CASE WHEN {IsCountrySubject} THEN 1 ELSE 0 END,
                     CASE WHEN {IsCountrySubject} THEN 0 ELSE SubjectID END,
                     CAST(SubjectName AS nvarchar(300))
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
            """, args);
        return (items, total);
    }

    private static string Like(string s) =>
        (s ?? "").Trim().Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");

    public Task<bool> SaveAsync(Subject subject) =>
        ProcReturnsRowAsync("SaveSubject", new { subject.SubjectName, subject.SubjectDetail });
}
