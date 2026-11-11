using System.Globalization;

namespace Nexam.Infrastructure;

/// <summary>
/// Subjects imported by country are named "COUNTRY - BODY - PROFESSION" (e.g. "UAE - DHA - NURSE").
/// The text before the first " - " is the country group; every other subject belongs to "General".
/// </summary>
public static class SubjectGroups
{
    public const string General = "General";
    private const string Separator = " - ";

    /// <summary>The group key a subject belongs to (also used as the filter value).</summary>
    public static string Of(string subjectName)
    {
        var name = (subjectName ?? "").Trim();
        var at = name.IndexOf(Separator, StringComparison.Ordinal);
        return at > 0 ? name[..at].Trim() : General;
    }

    /// <summary>Readable label for a group key: acronyms (UAE, UK, USA) stay upper-case, other names become Title Case.</summary>
    public static string Label(string group) =>
        group.Length <= 3 || group == General ? group : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(group.ToLowerInvariant());
}
