namespace Kysect.Tamgly.Core;

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

    public IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval()
    {
        var result = new List<WorkItemDeadline>();
        for (var currentMonth = new TamglyMonth(_interval.Start); currentMonth.Start < _interval.End; currentMonth = currentMonth.AddMonths(_period))
        {
            DateOnly currentDay = currentMonth.GetDateWith(_selectedDay);

            if (!_interval.Contains(currentDay))
                continue;

            result.Add(new WorkItemDeadline(new TamglyDay(currentDay)));
        }

        return result;
    }
}