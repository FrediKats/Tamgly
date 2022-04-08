using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class WorkItemDeadlineMonth : IWorkItemDeadline, IEquatable<WorkItemDeadlineMonth>
{
    private readonly TamglyMonth _tamglyMonth;

    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Month;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public WorkItemDeadlineMonth(TamglyMonth tamglyMonth)
    {
        _tamglyMonth = tamglyMonth;
        Number = _tamglyMonth.Number;
        Start = _tamglyMonth.Start;
        End = _tamglyMonth.End;
    }

    public static WorkItemDeadlineMonth FromDate(DateOnly dateTime)
    {
        return new WorkItemDeadlineMonth(new TamglyMonth(dateTime));
    }

    public bool Equals(WorkItemDeadlineMonth? other)
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
        return obj is WorkItemDeadlineMonth month && Equals(month);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}