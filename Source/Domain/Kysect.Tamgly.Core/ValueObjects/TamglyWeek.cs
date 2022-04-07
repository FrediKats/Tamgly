namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct TamglyWeek : IEquatable<TamglyWeek>
{
    private const int DayInWeek = 7;

    public int WeekNumber { get; }
    public DateOnly Start => TamglyTime.ZeroDay.AddDays(DayInWeek * WeekNumber);
    public DateOnly End => TamglyTime.ZeroDay.AddDays(DayInWeek * (WeekNumber + 1));

    public TamglyWeek(int weekNumber)
    {
        WeekNumber = weekNumber;
    }

    public static TamglyWeek FromDate(DateOnly dateTime)
    {
        var weekCount = TamglyTime.ZeroDay.DaysTo(dateTime) / DayInWeek;
        return new TamglyWeek(weekCount);
    }

    public bool Contains(DateOnly dateTime)
    {
        return Start <= dateTime && dateTime <= End;
    }

    public bool Equals(TamglyWeek other)
    {
        return WeekNumber == other.WeekNumber;
    }

    public override bool Equals(object? obj)
    {
        return obj is TamglyWeek other && Equals(other);
    }

    public override int GetHashCode()
    {
        return WeekNumber;
    }
}