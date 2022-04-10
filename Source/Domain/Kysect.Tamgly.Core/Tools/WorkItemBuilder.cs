using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Tools;

public class WorkItemBuilder
{
    private Guid _id;
    private string _title;
    private string? _description;
    private WorkItemState _state;
    private DateTime _creationTime;
    private ICollection<WorkItemTrackInterval> _intervals;
    private TimeSpan? _estimate;
    private WorkItemDeadline _deadline;

    public WorkItemBuilder(string title)
    {
        _id = Guid.NewGuid();
        _title = title;
        _description = null;
        _state = WorkItemState.Open;
        _creationTime = DateTime.Today;
        _intervals = new List<WorkItemTrackInterval>();
        _estimate = null;
        _deadline = WorkItemDeadline.NoDeadline;
    }

    public WorkItemBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public WorkItemBuilder SetDeadline(WorkItemDeadline deadline)
    {
        _deadline = deadline;
        return this;
    }

    public WorkItemBuilder SetEstimates(TimeSpan estimates)
    {
        _estimate = estimates;
        return this;
    }

    public WorkItem Build()
    {
        return new WorkItem(
            _id,
            _title,
            _description,
            _state,
            _creationTime,
            _intervals,
            _estimate,
            _deadline);
    }
}