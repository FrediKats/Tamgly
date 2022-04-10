using Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

namespace Kysect.Tamgly.Core.Tools;

public class RepetitiveParentWorkItemBuilder
{
    private Guid _id;
    private string _title;
    private readonly IRepetitiveInterval _repetitiveInterval;
    private string? _description;
    private DateTime _creationTime;
    private TimeSpan? _estimate;

    public RepetitiveParentWorkItemBuilder(string title, IRepetitiveInterval repetitiveInterval)
    {
        _id = Guid.NewGuid();
        _title = title;
        _repetitiveInterval = repetitiveInterval;
        _description = null;
        _creationTime = DateTime.Today;
        _estimate = null;
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

    public RepetitiveParentWorkItem Build()
    {
        return new RepetitiveParentWorkItem(
            _id,
            _title,
            _description,
            _creationTime,
            _estimate,
            _repetitiveInterval);
    }
}