using System.Diagnostics;
using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

[DebuggerDisplay("{ToShortString()}")]
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
    public WorkItemPriority? Priority { get; }

    public RepetitiveChildWorkItem(RepetitiveParentWorkItem parent, WorkItemDeadline deadline)
    {
        _parent = parent;
        Id = Guid.NewGuid();
        State = WorkItemState.Open;
        Intervals = new List<WorkItemTrackInterval>();
        Deadline = deadline;
        AssignedTo = _parent.AssignedTo;
        Priority = parent.Priority;
    }

    public string ToShortString()
    {
        return $"{GetType().Name}: {Title} ({Id.ToShortString()}), State: {State}";
    }
}