using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.ValueObjects;

public class WorkItemDeadline : IEquatable<WorkItemDeadline>
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();

    private readonly IDeadlineInterval? _deadlineInterval;

    private WorkItemDeadline()
    {
        _deadlineInterval = null;
    }

    private WorkItemDeadline(IDeadlineInterval deadlineInterval)
    {
        _deadlineInterval = deadlineInterval;
    }

    public static WorkItemDeadline Create(WorkItemDeadlineType type, DateOnly dateTime)
    {
        switch (type)
        {
            case WorkItemDeadlineType.Day:
                return new WorkItemDeadline(TamglyDay.FromDate(dateTime));

            case WorkItemDeadlineType.Week:
                return new WorkItemDeadline(TamglyWeek.FromDate(dateTime));

            case WorkItemDeadlineType.Month:
                return new WorkItemDeadline(TamglyMonth.FromDate(dateTime));

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
        if (_deadlineInterval is null)
            throw new TamglyException($"Cannot count days before deadline. Deadline type is {WorkItemDeadlineType.NoDeadline}");

        DateOnly firstDay = TamglyTime.MaxOf(_deadlineInterval.Start, TamglyTime.TodayDate);
        int daysBeforeDeadlineCount = firstDay.DaysTo(_deadlineInterval.End);
        return Math.Max(daysBeforeDeadlineCount, 0);
    }

    public override int GetHashCode()
    {
        return (_deadlineInterval != null ? _deadlineInterval.GetHashCode() : 0);
    }

    public bool Equals(WorkItemDeadline? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(_deadlineInterval, other._deadlineInterval);
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