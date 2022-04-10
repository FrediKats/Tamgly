using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WorkItem : IEquatable<WorkItem>, IWorkItem
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkItemState State { get; set; }
    public DateTime CreationTime { get; }
    public virtual ICollection<WorkItemTrackInterval> Intervals { get; }
    public TimeSpan? Estimate { get; private set; }
    public WorkItemDeadline Deadline { get; set; }
    public Person AssignedTo { get; set; }

    public WorkItem(Guid id, string title, string? description, WorkItemState state, DateTime creationTime, ICollection<WorkItemTrackInterval> intervals, TimeSpan? estimate, WorkItemDeadline deadline, Person assignedTo)
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
    }

    public void UpdateInfo(string title, string? description = null)
    {
        Title = title;
        Description = description;
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
}