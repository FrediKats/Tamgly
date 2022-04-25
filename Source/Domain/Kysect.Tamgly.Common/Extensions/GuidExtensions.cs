namespace Kysect.Tamgly.Common.Extensions;

public static class GuidExtensions
{
    public static string ToShortString(this Guid value)
    {
        return value.ToString().Substring(0, 5);
    }
}