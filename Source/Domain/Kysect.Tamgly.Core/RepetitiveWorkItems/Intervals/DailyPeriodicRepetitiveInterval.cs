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
        var result = new List<WorkItemDeadline>();
        for (DateOnly current = _interval.Start; current < _interval.End; current = current.AddDays(_period))
            result.Add(new WorkItemDeadline(new TamglyDay(current)));

        return result;
    }
}