namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class RepetitiveParentWorkItem
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationTime { get; }
    public TimeSpan? Estimate { get; private set; }
    public IRepetitiveInterval RepetitiveInterval { get; set; }
    public Person AssignedTo { get; }

    public RepetitiveParentWorkItem(Guid id, string title, string? description, DateTime creationTime, TimeSpan? estimate, IRepetitiveInterval repetitiveInterval, Person assignedTo)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationTime = creationTime;
        Estimate = estimate;
        RepetitiveInterval = repetitiveInterval;
        AssignedTo = assignedTo;
    }

    public IReadOnlyCollection<RepetitiveChildWorkItem> GetChildWorkItems()
    {
        return RepetitiveInterval
            .EnumeratePointOnInterval()
            .Select(d => new RepetitiveChildWorkItem(this, d))
            .ToList();
    }
}