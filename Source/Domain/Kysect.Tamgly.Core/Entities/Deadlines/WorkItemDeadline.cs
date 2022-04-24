using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.Deadlines;

public class WorkItemDeadline
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();

    private readonly ITimeInterval? _timeInterval;
    private readonly WorkItemDeadlineType _deadlineType;

    public WorkItemDeadline()
    {
        _timeInterval = null;
        _deadlineType = WorkItemDeadlineType.NoDeadline;
    }

    public WorkItemDeadline(TamglyDay day)
    {
        _timeInterval = day;
        _deadlineType = WorkItemDeadlineType.Day;
    }

    public WorkItemDeadline(TamglyWeek week)
    {
        _timeInterval = week;
        _deadlineType = WorkItemDeadlineType.Week;
    }

    public WorkItemDeadline(TamglyMonth month)
    {
        _timeInterval = month;
        _deadlineType = WorkItemDeadlineType.Month;
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (_deadlineType != other._deadlineType)
            return false;
        if (_timeInterval is null)
            return other._timeInterval is null;
        return _timeInterval.Equals(other._timeInterval);
    }

    public int GetDaysBeforeDeadlineCount()
    {
        if (_timeInterval is null)
            throw new TamglyException($"Cannot count days before deadline. Deadline type is {WorkItemDeadlineType.NoDeadline}");

        DateOnly firstDay = TamglyTime.MaxOf(_timeInterval.Start, TamglyTime.TodayDate);
        int daysBeforeDeadlineCount = firstDay.DaysTo(_timeInterval.End);
        return Math.Max(daysBeforeDeadlineCount, 0);
    }
}