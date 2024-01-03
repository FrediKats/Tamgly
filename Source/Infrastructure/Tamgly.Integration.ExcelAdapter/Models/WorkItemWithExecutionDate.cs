using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.ExecutionOrdering;
using Tamgly.Core.WorkItems;

namespace Tamgly.Integration.ExcelAdapter.Models;

public record WorkItemWithExecutionDate(DateOnly Date, WorkItem WorkItem)
{
    public static IReadOnlyCollection<WorkItemWithExecutionDate> From(ExecutionOrderItem orderItem)
    {
        return orderItem.WorkItems
            .Select(w => new WorkItemWithExecutionDate(orderItem.Date, w))
            .ToList();
    }
}