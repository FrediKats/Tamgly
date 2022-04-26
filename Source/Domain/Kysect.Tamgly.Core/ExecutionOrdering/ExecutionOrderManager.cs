using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public class ExecutionOrderManager : IExecutionOrderManager
{
    private readonly DateOnly _currentDay;

    public ExecutionOrderManager(DateOnly currentDay)
    {
        _currentDay = currentDay;
    }

    public ExecutionOrder Order(IReadOnlyCollection<WorkItem> workItems, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay)
    {
        workItems = workItems.Where(wi => wi.State == WorkItemState.Open).ToList();
       
        var currentDay = new TamglyDay(workItems.GetEarliestStart());
        var lastDay = new TamglyDay(workItems.GetEarliestEnd());

        var executionOrderContext = new ExecutionOrderQueue();
        var executionOrderBuilder = new ExecutionOrderBuilder(_currentDay, selectedDayOfWeek, limitPerDay);

        do
        {
            var dailyWorkItemBacklog = DailyWorkItemBacklog.Create(workItems, currentDay.Value);

            foreach (WorkItemPriority workItemPriority in Enum.GetValues<WorkItemPriority>())
            {
                ProcessBacklog(dailyWorkItemBacklog.CurrentDay.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessBacklog(dailyWorkItemBacklog.CurrentWeek.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessBacklog(dailyWorkItemBacklog.CurrentMonth.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessQueue(currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
            }

            currentDay = currentDay.AddDays();
        } while (currentDay.Value <= lastDay.Value || executionOrderContext.Any());

        return executionOrderBuilder.Build();
    }

    private void ProcessBacklog(
        ICollection<WorkItem> workItems,
        TamglyDay currentDay,
        ExecutionOrderBuilder executionOrderBuilder,
        ExecutionOrderQueue executionOrderQueue,
        WorkItemPriority priority)
    {
        foreach (WorkItem workItem in workItems)
        {
            if (workItem.Estimate is null)
                continue;

            if (workItem.Priority is null)
                continue;
            
            if (workItem.Priority.Value != priority)
                continue;

            ExecutionOrderItem assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date > currentDay.Value)
            {
                executionOrderQueue.Add(workItem);
            }
            else
            {
                assignments.WorkItems.Add(workItem);
            }
        }
    }

    private void ProcessQueue(
        TamglyDay currentDay,
        ExecutionOrderBuilder executionOrderBuilder,
        ExecutionOrderQueue executionOrderQueue,
        WorkItemPriority priority)
    {
        do
        {
            if (!executionOrderQueue.TryPeek(priority, out var workItem))
                return;

            if (workItem is null || workItem.Estimate is null)
                throw new TamglyException($"Invalid return result from ExecutionOrderQueue");

            ExecutionOrderItem assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date > currentDay.Value)
                return;

            workItem = executionOrderQueue.Dequeue(priority);
            assignments.WorkItems.Add(workItem);
        } while (true);
    }
}