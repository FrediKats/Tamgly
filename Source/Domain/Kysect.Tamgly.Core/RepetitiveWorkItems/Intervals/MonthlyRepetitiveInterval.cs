namespace Kysect.Tamgly.Core;

public class MonthlyRepetitiveInterval : IRepetitiveInterval
{
    private readonly TimeInterval _interval;
    private readonly int _period;

    public MonthlyRepetitiveInterval(TimeInterval interval, int period)
    {
        _interval = interval;
        _period = period;
    }

    public IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval()
    {
        var result = new List<WorkItemDeadline>();
        for (var currentMonth = new TamglyMonth(_interval.Start); currentMonth.Start < _interval.End; currentMonth = currentMonth.AddMonths(_period))
        {
            if (_interval.Contains(currentMonth.Start) || _interval.Contains(currentMonth.End))
                result.Add(new WorkItemDeadline(currentMonth));
        }

        return result;
    }
}