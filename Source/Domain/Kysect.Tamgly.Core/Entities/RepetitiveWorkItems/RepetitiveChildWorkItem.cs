using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public class RepetitiveChildWorkItem : IWorkItem
{
    private readonly RepetitiveParentWorkItem _parent;

    public Guid Id { get; }
    public string Title => _parent.Title;
    public string? Description => _parent.Description;
    public WorkItemState State { get; set; }
    public DateTime CreationTime => _parent.CreationTime;
    public ICollection<WorkItemTrackInterval> Intervals { get; }
    public TimeSpan? Estimate => _parent.Estimate;
    public WorkItemDeadline Deadline { get; set; }
    public Person AssignedTo { get; set; }

    public RepetitiveChildWorkItem(RepetitiveParentWorkItem parent, WorkItemDeadline deadline)
    {
        _parent = parent;
        Id = Guid.NewGuid();
        State = WorkItemState.Open;
        Intervals = new List<WorkItemTrackInterval>();
        Deadline = deadline;
        AssignedTo = _parent.AssignedTo;
    }
}