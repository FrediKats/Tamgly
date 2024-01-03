using Kysect.CommonLib.Collections.CollectionFiltering;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering.Filters;

public class WorkItemIsOpenFilterCondition : ICollectionFilterCondition<WorkItem>
{
    public static WorkItemIsOpenFilterCondition Instance { get; } = new WorkItemIsOpenFilterCondition();

    public FilteringResult<WorkItem> IsSatisfied(WorkItem element)
    {
        if (element.State != WorkItemState.Open)
            return new FilteringResult<WorkItem>(false, "Work items is not in open state.");

        return FilteringResult<WorkItem>.Ok();
    }
}