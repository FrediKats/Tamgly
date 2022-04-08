using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class TamglyDay : ITimeInterval
{
    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Day;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyDay(int number)
    {
        Number = number;
        Start = TamglyTime.ZeroDay.AddDays(Number);
        End = Start.AddDays(1);
    }

    public static TamglyDay FromDate(DateOnly dateTime)
    {
        return new TamglyDay(TamglyTime.ZeroDay.DaysTo(dateTime));
    }

    public bool Equals(TamglyMonth other)
    {
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        return obj is TamglyMonth other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}