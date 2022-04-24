using System.Diagnostics;
using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

[DebuggerDisplay("{ToShortString()}")]
public class WorkItem : IEquatable<WorkItem>
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkItemState State { get; set; }
    public DateTime CreationTime { get; }
    public virtual ICollection<WorkItemTrackInterval> Intervals { get; }
    public TimeSpan? Estimate { get; }
    public WorkItemDeadline Deadline { get; set; }
    public Person AssignedTo { get; }
    public WorkItemPriority? Priority { get; }


    public WorkItem(
        Guid id,
        string title,
        string? description,
        WorkItemState state,
        DateTime creationTime,
        ICollection<WorkItemTrackInterval> intervals,
        TimeSpan? estimate,
        WorkItemDeadline deadline,
        Person assignedTo,
        WorkItemPriority? priority)
    {
        Id = id;
        Title = title;
        Description = description;
        State = state;
        CreationTime = creationTime;
        Intervals = intervals;
        Estimate = estimate;
        Deadline = deadline;
        AssignedTo = assignedTo;
        Priority = priority;
    }

    public WorkItem(RepetitiveParentWorkItem parent, WorkItemDeadline deadline)
    {
        Id = Guid.NewGuid();
        Title = parent.Title;
        Description = parent.Description;
        State = WorkItemState.Open;
        CreationTime = parent.CreationTime;
        Intervals = new List<WorkItemTrackInterval>();
        Estimate = parent.Estimate;
        Deadline = deadline;
        AssignedTo = parent.AssignedTo;
        Priority = parent.Priority;
    }

    public void SetCompleted()
    {
        if (State != WorkItemState.Open)
            throw new TamglyException($"Work item already closed. Id: {Id}");

        State = WorkItemState.Closed;
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

    public bool Equals(WorkItem? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is WorkItem item)
            return Equals(item);
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void Deconstruct(
        out Guid id,
        out string title,
        out string? description,
        out WorkItemState state,
        out DateTime creationTime,
        out ICollection<WorkItemTrackInterval> intervals,
        out TimeSpan? estimate,
        out WorkItemDeadline deadline,
        out Person assignedTo,
        out WorkItemPriority? priority)
    {
        id = Id;
        title = Title;
        description = Description;
        state = State;
        creationTime = CreationTime;
        intervals = Intervals;
        estimate = Estimate;
        deadline = Deadline;
        assignedTo = AssignedTo;
        priority = Priority;
    }
}