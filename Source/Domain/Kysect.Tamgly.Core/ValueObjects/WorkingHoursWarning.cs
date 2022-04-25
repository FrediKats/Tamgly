using Kysect.Tamgly.Core.Entities.TimeIntervals;

namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct WorkingHoursWarning
{
    public ITimeInterval Interval { get; }
    public TimeSpan WorkingHoursLimit { get; }
    public TimeSpan CurrentEstimateSum { get; }

    public WorkingHoursWarning(ITimeInterval interval, TimeSpan workingHoursLimit, TimeSpan currentEstimateSum)
    {
        Interval = interval;
        WorkingHoursLimit = workingHoursLimit;
        CurrentEstimateSum = currentEstimateSum;
    }

    public string GetMessage()
    {
        return $"Interval {Interval} has {CurrentEstimateSum} hours in WI while limit is {WorkingHoursLimit}";
    }
}