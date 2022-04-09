using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public interface IWorkItem
{
    Guid Id { get; }
    string Title { get; }
    string? Description { get; }
    WorkItemState State { get; }
    DateTime CreationTime { get; }
    ICollection<WorkItemTrackInterval> Intervals { get; }
    TimeSpan? Estimate { get; }
    WorkItemDeadline Deadline { get; }
}

public static class WorkItemExtensions
{
    public static TimeSpan GetIntervalSum(this IWorkItem item)
    {
        TimeSpan result = TimeSpan.Zero;

        foreach (WorkItemTrackInterval interval in item.Intervals)
        {
            TimeSpan? duration = interval.GetDuration();
            if (duration is null)
                continue;

            result = result.Add(duration.Value);

        }

        return result;
    }

    public static double? TryGetEstimateMatchPercent(this IWorkItem item)
    {
        TimeSpan intervalSum = item.GetIntervalSum();
        if (item.Estimate is null)
            return null;

        double minValue = Math.Min(item.Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        double maxValue = Math.Max(item.Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        return minValue / maxValue;
    }
}