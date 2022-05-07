namespace Kysect.Tamgly.Core;

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

    public IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval()
    {
        var result = new List<WorkItemDeadline>();
        for (var currentWeekStart = new TamglyWeek(_interval.Start); currentWeekStart.Start < _interval.End; currentWeekStart = currentWeekStart.AddWeek(_period))
        {
            foreach (DateOnly currentDay in currentWeekStart.EnumerateDays())
            {
                if (!_interval.Contains(currentDay))
                    continue;

                if (_selectedDayOfWeek.Contains(currentDay))
                    result.Add(new WorkItemDeadline(new TamglyDay(currentDay)));
            }
        }

        return result;
    }
}