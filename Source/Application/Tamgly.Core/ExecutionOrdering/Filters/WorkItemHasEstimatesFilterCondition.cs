using Kysect.CommonLib.Collections.CollectionFiltering;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering.Filters;

public class WorkItemHasEstimatesFilterCondition : ICollectionFilterCondition<WorkItem>
{
    public static WorkItemHasEstimatesFilterCondition Instance { get; } = new WorkItemHasEstimatesFilterCondition();

    public FilteringResult<WorkItem> IsSatisfied(WorkItem element)
    {
        if (element.Estimate is null)
            return new FilteringResult<WorkItem>(false, "Work items does not have estimates.");

        return FilteringResult<WorkItem>.Ok();
    }
}