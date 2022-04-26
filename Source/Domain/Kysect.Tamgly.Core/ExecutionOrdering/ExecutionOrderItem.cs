using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public record ExecutionOrderItem(DateOnly Date, List<WorkItem> WorkItems)
{
    public TimeSpan TotalEstimates()
    {
        return WorkItems.Sum(item => item.Estimate) ?? TimeSpan.Zero;
    }

    public void Add(WorkItem workItem)
    {
        WorkItems.Add(workItem);
    }
}