namespace Kysect.Tamgly.Core;

public class DailyPeriodicRepetitiveInterval : IRepetitiveInterval
{
    private readonly ITimeInterval _interval;
    private readonly int _period;

    public DailyPeriodicRepetitiveInterval(ITimeInterval interval, int period)
    {
        _interval = interval;
        _period = period;
    }

    public IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval()
    {
        return TamglyDay
            .EnumerateDays(_interval, _period)
            .Select(d => new WorkItemDeadline(d))
            .ToList();
    }
}