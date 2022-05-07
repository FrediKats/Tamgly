namespace Kysect.Tamgly.Common;

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

    public static TimeSpan? Sum<T>(this IEnumerable<T> values, Func<T, TimeSpan?> selector)
    {
        TimeSpan? result = null;

        foreach (T value in values)
        {
            if (value is null)
                continue;

            TimeSpan? selectedValue = selector(value);
            if (selectedValue is null)
                continue;

            result = result?.Add(selectedValue.Value) ?? selectedValue;
        }

        return result;
    }
}