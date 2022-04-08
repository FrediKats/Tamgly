using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyEachWeekRepetitiveInterval : IRepetitiveInterval
{
    private readonly TimeInterval _interval;
    private readonly int _period;
    private readonly SelectedDayOfWeek _selectedDayOfWeek;

    public DailyEachWeekRepetitiveInterval(TimeInterval interval, int period, SelectedDayOfWeek selectedDayOfWeek)
    {
        _interval = interval;
        _period = period;
        _selectedDayOfWeek = selectedDayOfWeek;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly currentWeekStart = TamglyTime.GetWeekStart(_interval.Start); currentWeekStart < _interval.End; currentWeekStart = currentWeekStart.AddDays(_period * 7))
        {
            for (int dayOfWeekIndex = 0; dayOfWeekIndex < 7; dayOfWeekIndex++)
            {
                var currentDay = currentWeekStart.AddDays(dayOfWeekIndex);

                if (!_interval.Contains(currentDay))
                    continue;
                
                if (_selectedDayOfWeek.Contains(currentDay))
                    result.Add(currentDay);
            }
        }

        return result;
    }
}