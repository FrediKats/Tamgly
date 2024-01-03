using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.WorkItems;
using Tamgly.DataAccess.Models;

namespace Tamgly.Mapping.Implementations;

public class WorkItemMapper : IMapper<WorkItem, WorkItemDatabaseRecord>
{
    private readonly WorkItemTrackIntervalMapper _trackIntervalMapper;

    public WorkItemMapper(WorkItemTrackIntervalMapper trackIntervalMapper)
    {
        _trackIntervalMapper = trackIntervalMapper;
    }

    public WorkItemDatabaseRecord Map(WorkItem value)
    {
        return new WorkItemDatabaseRecord(
            value.Id,
            value.ExternalId,
            value.Title,
            value.Description,
            value.State,
            value.CreationTime,
            value.LastModifiedTime,
            value.CompletedTime,
            value.Estimate,
            value.Deadline.DeadlineType,
            value.Deadline.TimeInterval?.Start,
            value.AssignedTo,
            value.Priority);
    }

    public WorkItem Map(WorkItemDatabaseRecord value)
    {
        return new WorkItem(
            value.Id,
            value.ExternalId,
            value.Title,
            value.Description,
            value.State,
            value.CreationTime.ToLocalTime(),
            value.LastModifiedTime.ToLocalTime(),
            value.CompletedTime?.ToLocalTime(),
            value.Estimate,
            WorkItemDeadlineCreator.Create(value.DeadlineType, value.DeadlineTimeIntervalStart),
            value.AssignedTo,
            value.Priority);
    }
}