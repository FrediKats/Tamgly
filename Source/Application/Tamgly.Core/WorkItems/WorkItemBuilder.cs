using System;
using System.Collections.Generic;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.Backlogs.Deadlines;

namespace Tamgly.Core.WorkItems;

public class WorkItemBuilder
{
    private readonly IIdentifierGenerator _identifierGenerator;

    private string? _externalId;
    private readonly string _title;
    private string? _description;
    private WorkItemState _state;
    private DateTimeOffset _creationTime;
    private DateTimeOffset _lastModifiedTime;
    private DateTimeOffset? _completedTime;
    private ICollection<WorkItemTrackInterval> _intervals;
    private TimeSpan? _estimate;
    private WorkItemDeadline _deadline;
    private Person _assignedTo;
    private WorkItemPriority? _priority;

    public WorkItemBuilder(string title, IIdentifierGenerator identifierGenerator)
    {
        _identifierGenerator = identifierGenerator;

        _externalId = null;
        _title = title;
        _description = null;
        _state = WorkItemState.Open;
        _creationTime = DateTimeOffset.Now;
        _lastModifiedTime = DateTimeOffset.Now;
        _completedTime = null;
        _intervals = new List<WorkItemTrackInterval>();
        _estimate = null;
        _deadline = WorkItemDeadline.NoDeadline;
        _assignedTo = Person.Me;
        _priority = null;
    }

    public WorkItemBuilder SetExternalId(string? externalId)
    {
        _externalId = externalId;
        return this;
    }

    public WorkItemBuilder SetDescription(string? description)
    {
        _description = description;
        return this;
    }

    public WorkItemBuilder SetDeadline(WorkItemDeadline deadline)
    {
        _deadline = deadline;
        return this;
    }

    public WorkItemBuilder SetEstimates(TimeSpan? estimates)
    {
        _estimate = estimates;
        return this;
    }

    public WorkItemBuilder SetAssigning(Person person)
    {
        _assignedTo = person;
        return this;
    }

    public WorkItemBuilder SetPriority(WorkItemPriority? priority)
    {
        _priority = priority;
        return this;
    }

    public WorkItemBuilder SetCreationTime(DateTimeOffset creationTime)
    {
        _creationTime = creationTime;
        return this;
    }

    public WorkItemBuilder SetLastModifiedTime(DateTimeOffset lastModifiedTime)
    {
        _lastModifiedTime = lastModifiedTime;
        return this;
    }

    public WorkItemBuilder SetCompletedTime(DateTimeOffset? completedTime)
    {
        _completedTime = completedTime;
        return this;
    }

    public WorkItemBuilder SetState(WorkItemState state)
    {
        _state = state;
        return this;
    }

    public WorkItem Build()
    {
        var workItem = new WorkItem(
            _identifierGenerator.GetNext(),
            _externalId,
            _title,
            _description,
            _state,
            _creationTime,
            _lastModifiedTime,
            _completedTime,
            _estimate,
            _deadline,
            _assignedTo,
            _priority);

        return workItem;
    }
}