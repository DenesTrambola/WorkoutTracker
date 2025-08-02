namespace WorkoutTracker.Domain.Shared.Utilities;

using System.Text.RegularExpressions;

public static class DisplayNameHelper
{
    private static readonly Regex PascalCaseRegex = new(@"(?<!^)([A-Z])", RegexOptions.Compiled);

    public static string ToSentence(string pascalCase)
    {
        return PascalCaseRegex.Replace(pascalCase, " $1");
    }
}
