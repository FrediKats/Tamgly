using Kysect.Tamgly.Core.Entities.TimeIntervals;

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
        var result = new List<DateOnly>();
        for (var currentWeekStart = new TamglyWeek(_interval.Start); currentWeekStart.Start < _interval.End; currentWeekStart = currentWeekStart.AddWeek(_period))
        {
            foreach (DateOnly currentDay in currentWeekStart.EnumerateDays())
            {
                if (!_interval.Contains(currentDay))
                    continue;

                if (_selectedDayOfWeek.Contains(currentDay))
                    result.Add(currentDay);
            }
        }

        return result;
    }
}