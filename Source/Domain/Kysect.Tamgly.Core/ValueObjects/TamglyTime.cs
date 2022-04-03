namespace Kysect.Tamgly.Core.ValueObjects;

public static class TamglyTime
{
    public static readonly DateTime ZeroDay = new DateTime(2021, 12, 27);

    public static int DaysTo(this DateTime first, DateTime second)
    {
        TimeSpan intervalFromZeroDay = second.Subtract(first);
        return (int)intervalFromZeroDay.TotalDays;
    }
}