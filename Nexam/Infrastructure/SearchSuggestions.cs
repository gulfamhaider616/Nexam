namespace Nexam.Infrastructure;

public static class SearchSuggestions
{
    /// <summary>Collapses whitespace and trims long question text for a suggestion row.</summary>
    public static string Shorten(string text, int max = 140)
    {
        var s = string.Join(' ', (text ?? "").Split((char[])null, StringSplitOptions.RemoveEmptyEntries));
        return s.Length <= max ? s : s[..max].TrimEnd() + "…";
    }
}
