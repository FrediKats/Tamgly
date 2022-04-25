using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.TimeIntervals;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class WeeklyRepetitiveInterval : IRepetitiveInterval
{
    private readonly TimeInterval _interval;
    private readonly int _period;

    public WeeklyRepetitiveInterval(TimeInterval interval, int period)
    {
        _interval = interval;
        _period = period;
    }

    public IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval()
    {
        var result = new List<WorkItemDeadline>();
        for (var currentWeek = new TamglyWeek(_interval.Start); currentWeek.Start < _interval.End; currentWeek = currentWeek.AddWeek(_period))
        {
            if (_interval.Contains(currentWeek.Start) || _interval.Contains(currentWeek.End))
                result.Add(new WorkItemDeadline(currentWeek));
        }

        return result;
    }
}