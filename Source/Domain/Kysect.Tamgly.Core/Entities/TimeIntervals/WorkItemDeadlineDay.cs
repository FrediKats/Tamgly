using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class WorkItemDeadlineDay : IWorkItemDeadline, IEquatable<WorkItemDeadlineDay>
{
    private readonly TamglyDay _tamglyDay;

    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Day;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public WorkItemDeadlineDay(TamglyDay tamglyDay)
    {
        _tamglyDay = tamglyDay;

        Number = _tamglyDay.Number;
        Start = tamglyDay.Date;
        End = tamglyDay.Date;
    }

    public static WorkItemDeadlineDay FromDate(DateOnly dateTime)
    {
        return new WorkItemDeadlineDay(new TamglyDay(dateTime));
    }

    public override int GetHashCode()
    {
        return _tamglyDay.GetHashCode();
    }

    public bool Equals(WorkItemDeadlineDay? other)
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
        return obj is WorkItemDeadlineDay day && Equals(day);
    }
}