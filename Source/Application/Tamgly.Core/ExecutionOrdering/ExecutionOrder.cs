using Kysect.CommonLib.DateAndTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Common.DateAndTime;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering;

public class ExecutionOrder
{
    private readonly List<ExecutionOrderItem> _items;

    public IReadOnlyCollection<ExecutionOrderItem> Items => _items;

    public ExecutionOrder(IReadOnlyCollection<ExecutionOrderItem> items)
    {
        _items = items.ToList();
    }

    public ExecutionOrderItem GetPlaceForNewWorkItem(WorkItem workItem, TimeSpan timeLimit, SelectedDayOfWeek selectedDayOfWeek)
    {
        ExecutionOrderItem? executionOrderItem = _items.Find(i => i.CanAddMorePriorityWorkItem(workItem, timeLimit));

        if (executionOrderItem is not null)
            return executionOrderItem;

        DateOnly lastTimeWithTasks = _items.Last().Date;

        DateOnly nextDayInRange = lastTimeWithTasks.AddDays(1).NextDayInRange(selectedDayOfWeek);
        var orderItem = new ExecutionOrderItem(nextDayInRange, new List<WorkItem>());
        _items.Add(orderItem);
        return orderItem;
    }

}