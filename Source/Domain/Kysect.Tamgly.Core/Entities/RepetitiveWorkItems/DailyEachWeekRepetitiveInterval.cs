using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class DailyEachWeekRepetitiveInterval : IRepetitiveInterval
{
    private readonly DateOnly _startInterval;
    private readonly DateOnly _endInterval;
    private readonly int _period;
    private readonly SelectedDayOfWeek _selectedDayOfWeek;

    public DailyEachWeekRepetitiveInterval(DateOnly startInterval, DateOnly endInterval, int period, SelectedDayOfWeek selectedDayOfWeek)
    {
        _startInterval = startInterval;
        _period = period;
        _selectedDayOfWeek = selectedDayOfWeek;
        _endInterval = endInterval;
    }

    public IReadOnlyCollection<DateOnly> EnumeratePointOnInterval()
    {
        List<DateOnly> result = new List<DateOnly>();
        for (DateOnly currentWeekStart = TamglyTime.GetWeekStart(_startInterval); currentWeekStart < _endInterval; currentWeekStart = currentWeekStart.AddDays(_period * 7))
        {
            for (int dayOfWeekIndex = 0; dayOfWeekIndex < 7; dayOfWeekIndex++)
            {
                var currentDay = currentWeekStart.AddDays(dayOfWeekIndex);

                if (currentDay < _startInterval)
                    continue;
                
                if (currentDay > _endInterval)
                    continue;
                
                if (_selectedDayOfWeek.Contains(currentDay))
                    result.Add(currentDay);
            }
        }

        return result;
    }
}