using Kysect.CommonLib.Collections.CollectionFiltering;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.CommonLib.DateAndTime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Common.DateAndTime;
using Tamgly.Common.Exceptions;
using Tamgly.Common.WorkDayChecking;
using Tamgly.Core.Backlogs;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.ExecutionOrdering.Filters;
using Tamgly.Core.ExecutionOrdering.Models;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering;

public class ExecutionOrderManager : IExecutionOrderManager
{
    private readonly WorkDayChecker _workDayChecker;
    private readonly DateOnly _currentDay;
    private readonly TimeSpan _limitPerDay;
    private readonly ILogger _logger;

    public ExecutionOrderManager(DateOnly currentDay, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay, ILogger logger)
    {
        _currentDay = currentDay;
        _limitPerDay = limitPerDay;
        _logger = logger;
        _workDayChecker = new WorkDayChecker(HolidayList.Create(), selectedDayOfWeek);
    }

    public ExecutionOrder Order(IReadOnlyCollection<WorkItem> workItems)
    {
        _logger.LogInformation($"Start work items ordering. Work items {workItems.Count}");
        CollectionFilterApplier<WorkItem> collectionFilterApplier = ExecutionOrderInputCollectionFilter.CreateFilter(_logger);

        CollectionFilterApplyResult<WorkItem> collectionFilterApplyResult = collectionFilterApplier.Apply(workItems);
        workItems = collectionFilterApplyResult.SatisfiedElements;
        collectionFilterApplyResult.Reasons.LogIt();

        DateOnly? earliestEnd = workItems.GetEarliestEnd();
        var currentDay = new TamglyDay(_currentDay);
        var lastDay = new TamglyDay(_currentDay);

        if (earliestEnd is not null)
        {
            currentDay = new TamglyDay(DateOnlyExtensions.Min(earliestEnd.Value, _currentDay));
            lastDay = new TamglyDay(DateOnlyExtensions.Max(earliestEnd.Value, _currentDay));
        }

        var elementsWithoutDeadline = new ExecutionOrderQueue();
        var outdatedQueue = new ExecutionOrderQueue();
        var executionOrderBuilder = new ExecutionOrderBuilder(_currentDay, _limitPerDay, _logger, _workDayChecker);

        workItems
            .Where(wi => wi.Deadline.DeadlineType == WorkItemDeadlineType.NoDeadline)
            .ToList()
            .ForEach(elementsWithoutDeadline.Add);

        var dailyWorkItemBacklog = DailyWorkItemBacklog.Create(workItems, currentDay.Value);
        _logger.LogDebug($"Created WI backlog for items without deadline. Current date {currentDay}, Count {workItems.Count}");

        do
        {
            _logger.LogDebug($"Try to order items for {currentDay}");
            foreach (WorkItemPriority workItemPriority in Enum.GetValues<WorkItemPriority>())
            {
                _logger.LogDebug($"Try to add daily items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentDay.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                _logger.LogDebug($"Try to add weekly items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentWeek.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                _logger.LogDebug($"Try to add monthly items with priority {workItemPriority} to order");
                ProcessBacklog(dailyWorkItemBacklog.CurrentMonth.Items, currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                _logger.LogDebug($"Try to add items with priority {workItemPriority} from outdated items to order");
                ProcessQueue(currentDay, executionOrderBuilder, outdatedQueue, workItemPriority);
                _logger.LogDebug($"Try to add items without deadline with priority {workItemPriority} to order");
                ProcessQueue(currentDay, executionOrderBuilder, elementsWithoutDeadline, workItemPriority);
            }

            currentDay = currentDay.AddDays();
        }
        while (currentDay.Value <= lastDay.Value || elementsWithoutDeadline.Any() || outdatedQueue.Any());

        return executionOrderBuilder.Build();
    }

    public IReadOnlyCollection<ExecutionOrderDiff> GetDiffAfterAddingWorkItem(IReadOnlyCollection<WorkItem> workItems, WorkItem workItemForAdding)
    {
        ExecutionOrder before = Order(workItems);
        ExecutionOrder after = Order(workItems.Append(workItemForAdding).ToList());
        return GetDiff(before, after);
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
                _logger.LogDebug($"WI {workItem.ToShortString()} add to order {assignments.Date}");
                assignments.Add(workItem);
            }
            else
            {
                _logger.LogDebug($"WI {workItem.ToShortString()} cannot be added to order {assignments.Date}. Item will be added to queue");
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
                throw new TamglyException($"Invalid return result from ExecutionOrderQueue. Work item {workItem?.Id} does not have estimates.");

            ExecutionOrderItem assignments = executionOrderBuilder.GetDailyAssignmentsWithFreeTime(workItem.Estimate.Value);
            if (assignments.Date > currentDay.Value)
                return;

            _logger.LogDebug($"WI {workItem.ToShortString()} add to order {assignments.Date}");
            if (workItem != executionOrderQueue.Dequeue(priority))
                throw new TamglyException($"ExecutionOrderQueue return unexpected WI");
            assignments.Add(workItem);
        }
        while (true);
    }

    private IReadOnlyCollection<ExecutionOrderDiff> GetDiff(ExecutionOrder before, ExecutionOrder after)
    {
        var mapToExecutionDate = new Dictionary<int, DateOnly>();
        var result = new List<ExecutionOrderDiff>();

        before.Items
            .ForEach(i => i.WorkItems
                .ForEach(wi => mapToExecutionDate[wi.Id] = i.Date));

        after.Items
            .ForEach(i => i.WorkItems
                .ForEach(wi =>
                {
                    if (mapToExecutionDate.TryGetValue(wi.Id, out DateOnly dateBefore))
                    {
                        if (i.Date != dateBefore)
                            result.Add(new ExecutionOrderDiff(wi, dateBefore, i.Date));
                    }
                }));

        return result;
    }
}