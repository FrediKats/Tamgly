using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.Deadlines;

public class WorkItemDeadline
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();
    
    private readonly WorkItemDeadlineType _deadlineType;

    public ITimeInterval? TimeInterval { get; }

    public WorkItemDeadline()
    {
        TimeInterval = null;
        _deadlineType = WorkItemDeadlineType.NoDeadline;
    }

    public WorkItemDeadline(TamglyDay day)
    {
        TimeInterval = day;
        _deadlineType = WorkItemDeadlineType.Day;
    }

    public WorkItemDeadline(TamglyWeek week)
    {
        TimeInterval = week;
        _deadlineType = WorkItemDeadlineType.Week;
    }

    public WorkItemDeadline(TamglyMonth month)
    {
        TimeInterval = month;
        _deadlineType = WorkItemDeadlineType.Month;
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (_deadlineType != other._deadlineType)
            return false;
        if (TimeInterval is null)
            return other.TimeInterval is null;
        return TimeInterval.Equals(other.TimeInterval);
    }

    public int GetDaysBeforeDeadlineCount()
    {
        if (TimeInterval is null)
            throw new TamglyException($"Cannot count days before deadline. Deadline type is {WorkItemDeadlineType.NoDeadline}");

        DateOnly firstDay = TamglyTime.MaxOf(TimeInterval.Start, TamglyTime.TodayDate);
        int daysBeforeDeadlineCount = firstDay.DaysTo(TimeInterval.End);
        return Math.Max(daysBeforeDeadlineCount, 0);
    }
}