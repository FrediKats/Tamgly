using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.TimeTracking;
using Tamgly.Core.WorkItems;
using Tamgly.DataAccess;
using Tamgly.Mapping;

namespace Tamgly.Application;

public class WorkItemTrackingIntervalProvider : IWorkItemTrackingIntervalProvider
{
    private readonly TamglyDatabaseContext _databaseContext;
    private readonly MappingHolder _mappingHolder;

    public WorkItemTrackingIntervalProvider(TamglyDatabaseContext databaseContext, MappingHolder mappingHolder)
    {
        _databaseContext = databaseContext;
        _mappingHolder = mappingHolder;
    }

    public IReadOnlyCollection<WorkItemTrackInterval> GetIntervals(WorkItem workItem)
    {
        return _databaseContext.WorkItemsTrackingIntervals
            .GetForWorkItem(workItem.Id)
            .Select(i => _mappingHolder.WorkItemTrackInterval.Map(i))
            .ToList();
    }
}