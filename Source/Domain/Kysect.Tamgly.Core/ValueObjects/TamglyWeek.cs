namespace Kysect.Tamgly.Core.ValueObjects;

public struct TamglyWeek
{
    public int WeekNumber { get; }
    public DateTime Start => TamglyTime.ZeroDay.AddDays(7 * WeekNumber);
    public DateTime End => TamglyTime.ZeroDay.AddDays(7 * (WeekNumber +1)).AddTicks(-1);

    public TamglyWeek(int weekNumber)
    {
        WeekNumber = weekNumber;
    }

    public static TamglyWeek FromDate(DateTime dateTime)
    {
        var weekCount = TamglyTime.ZeroDay.DaysTo(dateTime) / 7;
        return new TamglyWeek(weekCount);
    }

    public bool Contains(DateTime dateTime)
    {
        return Start <= dateTime && dateTime <= End;
    }
}