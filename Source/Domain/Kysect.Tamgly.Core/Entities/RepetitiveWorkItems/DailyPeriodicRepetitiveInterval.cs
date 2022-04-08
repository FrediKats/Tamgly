namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyPeriodicRepetitiveInterval : IRepetitiveInterval
{
    private readonly DateOnly _startInterval;
    private readonly DateOnly _endInterval;
    private readonly int _period;

    public DailyPeriodicRepetitiveInterval(DateOnly startInterval, DateOnly endInterval, int period)
    {
        _startInterval = startInterval;
        _period = period;
        _endInterval = endInterval;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly current = _startInterval; current < _endInterval; current = current.AddDays(_period))
            result.Add(current);

        return result;
    }
}