using Kysect.CommonLib.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Common.Exceptions;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering;

public class ExecutionOrderItem
{
    private readonly List<WorkItem> _workItems;

    public DateOnly Date { get; init; }
    public IReadOnlyCollection<WorkItem> WorkItems => _workItems;

    public ExecutionOrderItem(DateOnly date, IReadOnlyCollection<WorkItem> workItems)
    {
        Date = date;
        _workItems = workItems.ToList();
    }

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
        _workItems.Add(workItem);
    }
}