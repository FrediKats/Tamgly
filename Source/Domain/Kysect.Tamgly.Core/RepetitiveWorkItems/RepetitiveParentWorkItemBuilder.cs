namespace Kysect.Tamgly.Core;

public class RepetitiveParentWorkItemBuilder
{
    private readonly IRepetitiveInterval _repetitiveInterval;

    private Guid _id;
    private string _title;
    private string? _description;
    private DateTime _creationTime;
    private TimeSpan? _estimate;
    private Person _assignedTo;
    private WorkItemPriority? _priority;

    public RepetitiveParentWorkItemBuilder(string title, IRepetitiveInterval repetitiveInterval)
    {
        _id = Guid.NewGuid();
        _title = title;
        _repetitiveInterval = repetitiveInterval;
        _description = null;
        _creationTime = DateTime.Today;
        _estimate = null;
        _assignedTo = Person.Me;
        _priority = null;
    }

    public RepetitiveParentWorkItemBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public RepetitiveParentWorkItemBuilder SetEstimates(TimeSpan estimates)
    {
        _estimate = estimates;
        return this;
    }

    public RepetitiveParentWorkItemBuilder SetAssigning(Person person)
    {
        _assignedTo = person;
        return this;
    }

    public RepetitiveParentWorkItemBuilder SetPriority(WorkItemPriority? priority)
    {
        _priority = priority;
        return this;
    }

    public RepetitiveParentWorkItem Build()
    {
        return new RepetitiveParentWorkItem(
            _id,
            _title,
            _description,
            _creationTime,
            _estimate,
            _repetitiveInterval,
            _assignedTo,
            _priority);
    }
}