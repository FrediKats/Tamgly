using System;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.WorkItems;

namespace Tamgly.DataAccess.Models;

public class WorkItemDatabaseRecord
{
    public int Id { get; init; }
    public string? ExternalId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public WorkItemState State { get; init; }
    public DateTimeOffset CreationTime { get; init; }
    public DateTimeOffset LastModifiedTime { get; init; }
    public DateTimeOffset? CompletedTime { get; init; }
    public TimeSpan? Estimate { get; init; }
    public WorkItemDeadlineType DeadlineType { get; }
    public DateOnly? DeadlineTimeIntervalStart { get; }
    public Person AssignedTo { get; init; }
    public WorkItemPriority? Priority { get; init; }

    public WorkItemDatabaseRecord(
        int id,
        string? externalId,
        string title,
        string? description,
        WorkItemState state,
        DateTimeOffset creationTime,
        DateTimeOffset lastModifiedTime,
        DateTimeOffset? completedTime,
        TimeSpan? estimate,
        WorkItemDeadlineType deadlineType,
        DateOnly? deadlineTimeIntervalStart,
        Person assignedTo,
        WorkItemPriority? priority) : this()
    {
        Id = id;
        ExternalId = externalId;
        Title = title;
        Description = description;
        State = state;
        CreationTime = creationTime;
        LastModifiedTime = lastModifiedTime;
        CompletedTime = completedTime;
        Estimate = estimate;
        DeadlineType = deadlineType;
        DeadlineTimeIntervalStart = deadlineTimeIntervalStart;
        AssignedTo = assignedTo;
        Priority = priority;
    }

    protected WorkItemDatabaseRecord()
    {
    }
}