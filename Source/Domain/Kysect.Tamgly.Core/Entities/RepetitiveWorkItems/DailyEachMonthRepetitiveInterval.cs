using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyEachMonthRepetitiveInterval : IRepetitiveInterval
{
    private readonly DateOnly _startInterval;
    private readonly DateOnly _endInterval;
    private readonly int _period;
    private readonly int _selectedDay;

    public DailyEachMonthRepetitiveInterval(DateOnly startInterval, DateOnly endInterval, int period, int selectedDay)
    {
        _startInterval = startInterval;
        _period = period;
        _selectedDay = selectedDay;
        _endInterval = endInterval;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly currentMonthStart = TamglyTime.GetMonthStart(_startInterval); currentMonthStart < _endInterval; currentMonthStart = currentMonthStart.AddMonths(_period))
        {
            DateOnly currentDay = currentMonthStart.ReplaceDayWith(_selectedDay);

            if (currentDay < _startInterval)
                continue;

            if (currentDay > _endInterval)
                continue;

            result.Add(currentDay);
        }

        return result;
    }
}