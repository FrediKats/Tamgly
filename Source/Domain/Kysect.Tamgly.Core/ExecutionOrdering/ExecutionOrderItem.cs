using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public record ExecutionOrderItem(DateOnly Date, List<WorkItem> WorkItems)
{
    public TimeSpan TotalEstimates()
    {
        return WorkItems.Sum(item => item.Estimate) ?? TimeSpan.Zero;
    }

    public bool CanAddMorePriorityWorkItem(WorkItem workItem, TimeSpan timeLimit)
    {
        if (workItem.Estimate is null)
            throw new TamglyException("Items in Execution order cannot be without estimates");

        TimeSpan totalEstimates = TimeSpan.Zero;

        foreach (WorkItem item in WorkItems)
        {
            if (item.Priority < workItem.Priority)
            {
                if (item.Estimate == null)
                    throw new TamglyException("Items in Execution order cannot be without estimates");

                totalEstimates = totalEstimates.Add(item.Estimate.Value);
            }
        }

        return totalEstimates + workItem.Estimate.Value < timeLimit;
    }

    public void Add(WorkItem workItem)
    {
        WorkItems.Add(workItem);
    }
}