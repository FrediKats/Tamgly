namespace Kysect.Tamgly.Core;

public class RepetitiveParentWorkItem
{
    private IReadOnlyCollection<WorkItem> _childrenWorkItems;
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationTime { get; }
    public TimeSpan? Estimate { get; private set; }
    public IRepetitiveInterval RepetitiveInterval { get; set; }
    public Person AssignedTo { get; }
    public WorkItemPriority? Priority { get; }

    public RepetitiveParentWorkItem(Guid id, string title, string? description, DateTime creationTime, TimeSpan? estimate, IRepetitiveInterval repetitiveInterval, Person assignedTo, WorkItemPriority? priority)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationTime = creationTime;
        Estimate = estimate;
        RepetitiveInterval = repetitiveInterval;
        AssignedTo = assignedTo;
        Priority = priority;

        _childrenWorkItems = RepetitiveInterval
            .EnumeratePointOnInterval()
            .Select(d => WorkItem.CreateFromRepetitive(this, d))
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetChildWorkItems()
    {
        return _childrenWorkItems;
    }
}