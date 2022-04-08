using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public class WorkItemDeadlineWeek : IWorkItemDeadline, IEquatable<WorkItemDeadlineWeek>
{
    private readonly TamglyWeek _tamglyWeek;

    public WorkItemDeadlineType DeadlineType => WorkItemDeadlineType.Week;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public WorkItemDeadlineWeek(TamglyWeek tamglyWeek)
    {
        _tamglyWeek = tamglyWeek;

        Number = _tamglyWeek.Number;
        Start = _tamglyWeek.Start;
        End = _tamglyWeek.End;
    }
    
    public static WorkItemDeadlineWeek FromDate(DateOnly dateTime)
    {
        return new WorkItemDeadlineWeek(new TamglyWeek(dateTime));
    }

    public bool Equals(WorkItemDeadlineWeek? other)
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
        return obj is WorkItemDeadlineWeek week && Equals(week);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}