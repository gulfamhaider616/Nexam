using Nexam.Models.Entities;

namespace Nexam.Business.Helpers;

internal static class QuestionHelper
{
    private static readonly char[] ChoiceSeparators = { ',', '،' };   // Latin and Arabic/Urdu comma

    /// <summary>Splits the stored comma-separated choices into options. Null when there are none.</summary>
    public static List<QuestionOption> ParseOptions(string choices)
    {
        if (string.IsNullOrEmpty(choices)) return null;
        return choices.Split(ChoiceSeparators).Select(c => new QuestionOption { Choice = c }).ToList();
    }

    public static List<Question> WithOptions(this List<Question> questions)
    {
        foreach (var question in questions)
            question.Options = ParseOptions(question.Choices);
        return questions;
    }
}
