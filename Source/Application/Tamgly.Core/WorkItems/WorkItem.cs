using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tamgly.Core.Backlogs.Deadlines;

namespace Tamgly.Core.WorkItems;

[DebuggerDisplay("{ToShortString()}")]
public class WorkItem
{
    public int Id { get; init; }
    public string? ExternalId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public WorkItemState State { get; init; }
    public DateTimeOffset CreationTime { get; init; }
    public DateTimeOffset LastModifiedTime { get; init; }
    public DateTimeOffset? CompletedTime { get; init; }
    public TimeSpan? Estimate { get; init; }
    public WorkItemDeadline Deadline { get; set; }
    public Person AssignedTo { get; init; }
    public WorkItemPriority? Priority { get; set; }

    public WorkItem(int id,
        string? externalId,
        string title,
        string? description,
        WorkItemState state,
        DateTimeOffset creationTime,
        DateTimeOffset lastModifiedTime,
        DateTimeOffset? completedTime,
        TimeSpan? estimate,
        WorkItemDeadline deadline,
        Person assignedTo,
        WorkItemPriority? priority)
    {
        Id = id;
        ExternalId = externalId;
        Title = title;
        Description = description;
        State = state;
        CreationTime = creationTime;
        LastModifiedTime = lastModifiedTime;
        CompletedTime = completedTime;
        Estimate = estimate;
        Deadline = deadline;
        AssignedTo = assignedTo;
        Priority = priority;
    }

    public TimeSpan GetIntervalSum(IReadOnlyCollection<WorkItemTrackInterval> intervals)
    {
        TimeSpan result = TimeSpan.Zero;

        foreach (WorkItemTrackInterval interval in intervals)
        {
            TimeSpan? duration = interval.GetDuration();
            if (duration is null)
                continue;

            result = result.Add(duration.Value);
        }

        return result;
    }

    public double? TryGetEstimateMatchPercent(IReadOnlyCollection<WorkItemTrackInterval> intervals)
    {
        TimeSpan intervalSum = GetIntervalSum(intervals);
        if (Estimate is null)
            return null;

        double minValue = Math.Min(Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        double maxValue = Math.Max(Estimate.Value.TotalMinutes, intervalSum.TotalMinutes);
        return minValue / maxValue;
    }

    public string ToShortString()
    {
        return $"{GetType().Name}: {Title} ({Id}), State: {State}";
    }

    public string ToLogString()
    {
        return ToShortString();
    }

    public override string ToString()
    {
        return ToShortString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}