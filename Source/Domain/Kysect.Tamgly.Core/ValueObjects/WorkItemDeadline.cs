using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.ValueObjects;

public class WorkItemDeadline
{
    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline();

    private readonly ITimeInterval? _timeInterval;

    public WorkItemDeadline()
    {
        _timeInterval = null;
    }

    public WorkItemDeadline(TamglyDay day)
    {
        _timeInterval = day;
    }

    public WorkItemDeadline(TamglyWeek week)
    {
        _timeInterval = week;
    }

    public WorkItemDeadline(TamglyMonth month)
    {
        _timeInterval = month;
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
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