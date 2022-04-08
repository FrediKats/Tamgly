using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class TamglyDay : ITimeInterval, IEquatable<TamglyDay>
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

    public override int GetHashCode()
    {
        return Number;
    }

    public bool Equals(TamglyDay? other)
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
        return obj is TamglyDay day && Equals(day);
    }
}