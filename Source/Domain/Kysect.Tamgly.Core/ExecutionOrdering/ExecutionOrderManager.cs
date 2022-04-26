using Kysect.Tamgly.Common;
using Serilog;

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
       
        var currentDay = new TamglyDay(DateOnlyExtensions.Min(workItems.GetEarliestEnd(), _currentDay));
        var lastDay = new TamglyDay(DateOnlyExtensions.Max(workItems.GetEarliestEnd(), _currentDay));

        var elementsWithoutDeadline = new ExecutionOrderQueue();
        var outdatedQueue = new ExecutionOrderQueue();
        var executionOrderBuilder = new ExecutionOrderBuilder(_currentDay, selectedDayOfWeek, limitPerDay);

        workItems
            .Where(wi => wi.Deadline.DeadlineType == WorkItemDeadlineType.NoDeadline)
            .ToList()
            .ForEach(elementsWithoutDeadline.Add);

        var dailyWorkItemBacklog = DailyWorkItemBacklog.Create(workItems, currentDay.Value);

        do
        {
            Log.Verbose($"Try to order items for {currentDay}");
            foreach (WorkItemPriority workItemPriority in Enum.GetValues<WorkItemPriority>())
            {
                Log.Verbose($"Try to add daily items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentDay.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                Log.Verbose($"Try to add weekly items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentWeek.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                Log.Verbose($"Try to add monthly items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentMonth.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                Log.Verbose($"Try to add items with priority {workItemPriority} from outdated items to order");
                ProcessQueue(currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                Log.Verbose($"Try to add items without deadline with priority {workItemPriority} to order");
                ProcessQueue(currentDay, executionOrderBuilder, elementsWithoutDeadline, workItemPriority);
            }

            currentDay = currentDay.AddDays();
        } while (currentDay.Value <= lastDay.Value || outdatedQueue.Any());

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

            if (executionOrderBuilder.IsAdded(workItem))
                continue;

            if (executionOrderQueue.IsAdded(workItem))
                continue;

            ExecutionOrderItem assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date <= currentDay.Value)
            {
                Log.Verbose($"WI {workItem.ToShortString()} add to order {assignments.Date}");
                assignments.Add(workItem);
            }
            else
            {
                Log.Verbose($"WI {workItem.ToShortString()} cannot be added to order {assignments.Date}. Item will be added to queue");
                executionOrderQueue.Add(workItem);
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
            if (!executionOrderQueue.TryPeek(priority, out WorkItem? workItem))
                return;

            if (workItem is null || workItem.Estimate is null)
                throw new TamglyException($"Invalid return result from ExecutionOrderQueue");

            ExecutionOrderItem assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date > currentDay.Value)
                return;

            Log.Verbose($"WI {workItem.ToShortString()} add to order {assignments.Date}");
            if (workItem != executionOrderQueue.Dequeue(priority))
                throw new TamglyException($"ExecutionOrderQueue return unexpected WI");
            assignments.Add(workItem);
        } while (true);
    }
}