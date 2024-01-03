using Kysect.CommonLib.Collections.CollectionFiltering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering.Filters;

public class ExecutionOrderInputCollectionFilter
{
    public static CollectionFilterApplier<WorkItem> CreateFilter(ILogger logger)
    {
        var filterConditions = new List<ICollectionFilterCondition<WorkItem>>
        {
            WorkItemIsOpenFilterCondition.Instance,
            WorkItemHasEstimatesFilterCondition.Instance
        };

        return new CollectionFilterApplier<WorkItem>(filterConditions, logger);
    }
}