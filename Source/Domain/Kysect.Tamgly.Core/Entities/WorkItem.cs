using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WorkItem : IEquatable<WorkItem>
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkItemState State { get; set; }
    public DateTime CreationTime { get; }
    public virtual ICollection<WorkItemTrackInterval> Intervals { get; }

    public static WorkItem Create(string title, string? description = null)
    {
        return new WorkItem(Guid.NewGuid(), title, description, WorkItemState.Open, DateTime.Now, new List<WorkItemTrackInterval>());
    }

    public WorkItem(Guid id, string title, string? description, WorkItemState state, DateTime creationTime, ICollection<WorkItemTrackInterval> intervals)
    {
        Id = id;
        Title = title;
        Description = description;
        State = state;
        CreationTime = creationTime;
        Intervals = intervals;
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