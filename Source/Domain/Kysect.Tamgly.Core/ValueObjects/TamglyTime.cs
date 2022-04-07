namespace Kysect.Tamgly.Core.ValueObjects;

public static class TamglyTime
{
    public static readonly DateOnly ZeroDay = new DateOnly(2021, 12, 27);

    public static int DaysTo(this DateOnly first, DateOnly second)
    {
        TimeSpan intervalFromZeroDay = second.ToDateTime(TimeOnly.MinValue).Subtract(first.ToDateTime(TimeOnly.MinValue));
        return (int)intervalFromZeroDay.TotalDays;
    }
}