using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyPeriodicRepetitiveInterval : IRepetitiveInterval
{
    private readonly TimeInterval _interval;

    private readonly int _period;

    public DailyPeriodicRepetitiveInterval(TimeInterval interval, int period)
    {
        _interval = interval;
        _period = period;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly current = _interval.Start; current < _interval.End; current = current.AddDays(_period))
            result.Add(current);

        return result;
    }
}