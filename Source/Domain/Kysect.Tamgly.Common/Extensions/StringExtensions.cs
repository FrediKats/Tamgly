namespace Kysect.Tamgly.Common.Extensions;

public static class StringExtensions
{
    public static string FromChar(char c, int count)
    {
        return string.Join("", Enumerable.Repeat(c, count));
    }
}