using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.ValueObjects;

public class WorkItemDeadline : IEquatable<WorkItemDeadline>
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();

    private readonly IWorkItemDeadline? _workItemDeadline;

    private WorkItemDeadline()
    {
        _workItemDeadline = null;
    }

    private WorkItemDeadline(IWorkItemDeadline workItemDeadline)
    {
        _workItemDeadline = workItemDeadline;
    }

    public static WorkItemDeadline Create(WorkItemDeadlineType type, DateOnly dateTime)
    {
        switch (type)
        {
            case WorkItemDeadlineType.Day:
                return new WorkItemDeadline(WorkItemDeadlineDay.FromDate(dateTime));

            case WorkItemDeadlineType.Week:
                return new WorkItemDeadline(WorkItemDeadlineWeek.FromDate(dateTime));

            case WorkItemDeadlineType.Month:
                return new WorkItemDeadline(WorkItemDeadlineMonth.FromDate(dateTime));

            case WorkItemDeadlineType.NoDeadline:
            default:
                throw new ArgumentException($"{type} is not acceptable type for deadline.");
        }
    }

    public bool MatchedWith(WorkItemDeadlineType type, DateOnly dateTime)
    {
        WorkItemDeadline other = Create(type, dateTime);
        return Equals(other);
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
        return Equals(other);
    }

    public int GetDaysBeforeDeadlineCount()
    {
        if (_workItemDeadline is null)
            throw new TamglyException($"Cannot count days before deadline. Deadline type is {WorkItemDeadlineType.NoDeadline}");

        DateOnly firstDay = TamglyTime.MaxOf(_workItemDeadline.Start, TamglyTime.TodayDate);
        int daysBeforeDeadlineCount = firstDay.DaysTo(_workItemDeadline.End);
        return Math.Max(daysBeforeDeadlineCount, 0);
    }

    public override int GetHashCode()
    {
        return (_workItemDeadline != null ? _workItemDeadline.GetHashCode() : 0);
    }

    public bool Equals(WorkItemDeadline? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(_workItemDeadline, other._workItemDeadline);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((WorkItemDeadline)obj);
    }
}