namespace Kysect.Tamgly.Core.ValueObjects;

public static class TamglyTime
{
    public static readonly DateOnly ZeroDay = new DateOnly(2021, 12, 27);
    public static readonly DateOnly ZeroMonth = ZeroDay.AddDays(-ZeroDay.Day);

    public static DateOnly TodayDate => DateOnly.FromDateTime(DateTime.UtcNow);

    public static int DaysTo(this DateOnly first, DateOnly second)
    {
        TimeSpan interval = second.ToDateTime(TimeOnly.MinValue).Subtract(first.ToDateTime(TimeOnly.MinValue));
        return (int)interval.TotalDays;
    }

    public static DateOnly MaxOf(DateOnly first, DateOnly second)
    {
        return first >= second ? first : second;
    }
}