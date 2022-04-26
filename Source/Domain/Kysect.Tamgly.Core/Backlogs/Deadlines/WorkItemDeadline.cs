namespace Kysect.Tamgly.Core;

public class WorkItemDeadline
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();

    public ITimeInterval? TimeInterval { get; }
    public WorkItemDeadlineType DeadlineType { get; }

    public WorkItemDeadline()
    {
        TimeInterval = null;
        DeadlineType = WorkItemDeadlineType.NoDeadline;
    }

    public WorkItemDeadline(TamglyDay day)
    {
        TimeInterval = day;
        DeadlineType = WorkItemDeadlineType.Day;
    }

    public WorkItemDeadline(TamglyWeek week)
    {
        TimeInterval = week;
        DeadlineType = WorkItemDeadlineType.Week;
    }

    public WorkItemDeadline(TamglyMonth month)
    {
        TimeInterval = month;
        DeadlineType = WorkItemDeadlineType.Month;
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (DeadlineType != other.DeadlineType)
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