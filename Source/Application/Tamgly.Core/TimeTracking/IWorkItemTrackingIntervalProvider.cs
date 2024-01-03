using System.Collections.Generic;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.TimeTracking;

public interface IWorkItemTrackingIntervalProvider
{
    IReadOnlyCollection<WorkItemTrackInterval> GetIntervals(WorkItem workItem);
}