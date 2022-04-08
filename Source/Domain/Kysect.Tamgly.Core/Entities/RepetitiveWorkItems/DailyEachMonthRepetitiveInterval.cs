using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyEachMonthRepetitiveInterval : IRepetitiveInterval
{
    private readonly TimeInterval _interval;
    private readonly int _period;
    private readonly int _selectedDay;

    public DailyEachMonthRepetitiveInterval(TimeInterval interval, int period, int selectedDay)
    {
        _interval = interval;
        _period = period;
        _selectedDay = selectedDay;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly currentMonthStart = TamglyTime.GetMonthStart(_interval.Start); currentMonthStart < _interval.End; currentMonthStart = currentMonthStart.AddMonths(_period))
        {
            DateOnly currentDay = currentMonthStart.ReplaceDayWith(_selectedDay);

            if (!_interval.Contains(currentDay))
                continue;

            result.Add(currentDay);
        }

        return result;
    }
}