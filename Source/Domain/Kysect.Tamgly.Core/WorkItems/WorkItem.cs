using System.Diagnostics;
using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

[DebuggerDisplay("{ToShortString()}")]
public record WorkItem(
    Guid Id,
    string Title,
    string? Description,
    WorkItemState State,
    DateTime CreationTime,
    ICollection<WorkItemTrackInterval> Intervals,
    TimeSpan? Estimate,
    WorkItemDeadline Deadline,
    Person AssignedTo,
    WorkItemPriority? Priority)
{
    public static WorkItem CreateFromRepetitive(RepetitiveParentWorkItem parent, WorkItemDeadline deadline)
    {
        return new WorkItem(
            Id: Guid.NewGuid(),
            parent.Title,
            parent.Description,
            WorkItemState.Open,
            parent.CreationTime,
            Intervals: new List<WorkItemTrackInterval>(),
            parent.Estimate,
            deadline,
            parent.AssignedTo,
            parent.Priority);

    }

    public void AddInterval(WorkItemTrackInterval interval)
    {
        ArgumentNullException.ThrowIfNull(interval);

        Intervals.Add(interval);
    }

    public TimeSpan GetIntervalSum()
    {
        TimeSpan result = TimeSpan.Zero;

        foreach (WorkItemTrackInterval interval in Intervals)
        {
            TimeSpan? duration = interval.GetDuration();
            if (duration is null)
                continue;

            result = result.Add(duration.Value);

        }

        return result;
    }

    public double? TryGetEstimateMatchPercent()
    {
        TimeSpan intervalSum = GetIntervalSum();
        if (Estimate is null)
            return null;

        double minValue = Math.Min(Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        double maxValue = Math.Max(Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        return minValue / maxValue;
    }

    public string ToShortString()
    {
        return $"{GetType().Name}: {Title} ({Id.ToShortString()}), State: {State}";
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}