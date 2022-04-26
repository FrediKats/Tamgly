using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public class ExecutionOrderManager : IExecutionOrderManager
{
    public void Order(List<WorkItem> workItems, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay)
    {
        workItems = workItems.Where(wi => wi.State == WorkItemState.Open).ToList();
        ILookup<WorkItemDeadlineType, WorkItem> lookup = workItems.ToLookup(wi => wi.Deadline.DeadlineType);
        List<WorkItem> wiForDay = lookup[WorkItemDeadlineType.Day].OrderBy(wi => wi.Deadline).ToList();
        ITimeInterval? firstWiDeadline = wiForDay.First().Deadline.TimeInterval;
        ITimeInterval? lastWiDeadline = wiForDay.Last().Deadline.TimeInterval;

        if (firstWiDeadline is null)
            throw new TamglyException($"WI {wiForDay.First().ToShortString()} has incorrect deadline.");

        if (lastWiDeadline is null)
            throw new TamglyException($"WI {wiForDay.Last().ToShortString()} has incorrect deadline.");

        var firstDay = firstWiDeadline.To<TamglyDay>();
        var lastDay = lastWiDeadline.To<TamglyDay>();

        var executionOrderContext = new ExecutionOrderQueue();
        var executionOrderBuilder = new ExecutionOrderBuilder(DateOnly.FromDateTime(DateTime.Now), selectedDayOfWeek, limitPerDay);

        TamglyDay currentDay = firstDay;
        do
        {
            var workItemPriorities = new[]
            {
                WorkItemPriority.P1,
                WorkItemPriority.P2,
                WorkItemPriority.P3,
                WorkItemPriority.P4,
                WorkItemPriority.P5
            };

            var dailyWorkItemBacklog = DailyWorkItemBacklog.Create(workItems, currentDay.Value);

            foreach (WorkItemPriority workItemPriority in workItemPriorities)
            {
                ProcessBacklog(dailyWorkItemBacklog.CurrentDay.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessBacklog(dailyWorkItemBacklog.CurrentWeek.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessBacklog(dailyWorkItemBacklog.CurrentMonth.Items, currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
                ProcessQueue(currentDay, executionOrderBuilder, executionOrderContext, workItemPriority);
            }

        } while (currentDay.Value <= lastDay.Value || executionOrderContext.Any());
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

            DailyAssignments assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
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

            DailyAssignments assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date > currentDay.Value)
                return;

            assignments.WorkItems.Add(workItem);
        } while (true);
    }
}