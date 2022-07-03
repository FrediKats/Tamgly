namespace Kysect.Tamgly.Common;

public static class StringExtensions
{
    public static string FromChar(char c, int count)
    {
        return string.Join(string.Empty, Enumerable.Repeat(c, count));
    }
}