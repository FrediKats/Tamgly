using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public record ExecutionOrder(List<ExecutionOrderItem> Items)
{
    public ExecutionOrderItem GetPlaceForNewWorkItem(WorkItem workItem, TimeSpan timeLimit, SelectedDayOfWeek selectedDayOfWeek)
    {
        ExecutionOrderItem? executionOrderItem = Items.Find(i => i.CanAddMorePriorityWorkItem(workItem, timeLimit));

        if (executionOrderItem is not null)
            return executionOrderItem;

        DateOnly lastTimeWithTasks = Items.Last().Date;

        DateOnly nextDayInRange = lastTimeWithTasks.AddDays(1).NextDayInRange(selectedDayOfWeek);
        var orderItem = new ExecutionOrderItem(nextDayInRange, new List<WorkItem>());
        Items.Add(orderItem);
        return orderItem;
    }
}