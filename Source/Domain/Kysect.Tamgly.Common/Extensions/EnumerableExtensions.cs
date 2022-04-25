namespace Kysect.Tamgly.Common.Extensions;

public static class EnumerableExtensions
{
    public static TimeSpan? Sum(this IEnumerable<TimeSpan?> values)
    {
        TimeSpan? result = null;

        foreach (TimeSpan? value in values)
        {
            if (value is null)
                continue;

            result = result?.Add(value.Value) ?? value;
        }

        return result;
    }
}