using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class TamglyMonth : ITimeInterval, IEquatable<TamglyMonth>
{
    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Month;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyMonth(int number)
    {
        Number = number;
        Start = TamglyTime.ZeroMonth.AddMonths(Number);
        End = Start.AddMonths(1);
    }

    public static TamglyMonth FromDate(DateOnly dateTime)
    {
        var monthNumber = 0;
        var currentTime = TamglyTime.ZeroMonth;

        while (currentTime < dateTime)
        {
            currentTime = currentTime.AddMonths(1);
            monthNumber++;
        }

        return new TamglyMonth(monthNumber);
    }

    public bool Equals(TamglyMonth? other)
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
        if (obj.GetType() != this.GetType())
            return false;
        return obj is TamglyMonth month && Equals(month);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}