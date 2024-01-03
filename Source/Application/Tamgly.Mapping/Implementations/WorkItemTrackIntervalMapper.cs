using Tamgly.Core.WorkItems;
using Tamgly.DataAccess.Models;

namespace Tamgly.Mapping.Implementations;

public class WorkItemTrackIntervalMapper : IMapper<WorkItemTrackInterval, WorkItemTrackIntervalDatabaseRecord>
{
    public WorkItemTrackIntervalDatabaseRecord Map(WorkItemTrackInterval value)
    {
        return new WorkItemTrackIntervalDatabaseRecord(
            value.Id,
            value.ActivityId,
            value.StartTime,
            value.EndTime);
    }

    public WorkItemTrackInterval Map(WorkItemTrackIntervalDatabaseRecord value)
    {
        return new WorkItemTrackInterval(
            value.Id,
            value.ActivityId,
            value.StartTime,
            value.EndTime);
    }
}