using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class TamglyWeek : ITimeInterval, IEquatable<TamglyWeek>
{
    private const int DayInWeek = 7;

    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Week;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyWeek(int number)
    {
        Number = number;
        Start = TamglyTime.ZeroDay.AddDays(DayInWeek * Number);
        End = Start.AddDays(DayInWeek);
    }

    public static TamglyWeek FromDate(DateOnly dateTime)
    {
        var weekCount = TamglyTime.ZeroDay.DaysTo(dateTime) / DayInWeek;
        return new TamglyWeek(weekCount);
    }

    public bool Equals(TamglyWeek? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj is TamglyWeek week && Equals(week);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}