using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Common.Exceptions;
using Tamgly.Common.WorkDayChecking;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering;

public class ExecutionOrderBuilder
{
    private readonly WorkDayChecker _workDayChecker;
    private readonly TimeSpan _workingHoursPerDay;
    private readonly List<ExecutionOrderItem> _assignedWorkItems;
    private readonly ILogger _logger;

    private DateOnly _currentDay;

    public ExecutionOrderBuilder(DateOnly currentDay, TimeSpan workingHoursPerDay, ILogger logger, WorkDayChecker workDayChecker)
    {
        _currentDay = currentDay;
        _workingHoursPerDay = workingHoursPerDay;
        _workDayChecker = workDayChecker;
        _logger = logger;
        _assignedWorkItems = new List<ExecutionOrderItem>();
    }

    public ExecutionOrder Build()
    {
        return new ExecutionOrder(_assignedWorkItems);
    }

    public ExecutionOrderItem GetDailyAssignmentsWithFreeTime(TimeSpan estimates)
    {
        if (estimates > _workingHoursPerDay)
            throw new TamglyException($"Cannot find time for Work item. Estimates is bigger that time for a one day.");

        if (!_assignedWorkItems.Any())
        {
            _logger.LogDebug($"No daily assignments exists. Create new for date: {_currentDay}");
            return CreateDailyAssignmentForNextDay();
        }

        ExecutionOrderItem lastAssignment = _assignedWorkItems.Last();
        if (lastAssignment.TotalEstimates() + estimates > _workingHoursPerDay)
        {
            _logger.LogDebug($"Cannot add WI with estimate {estimates} to daily assignments {lastAssignment.Date}. Not enough free time");
            return CreateDailyAssignmentForNextDay();
        }

        return lastAssignment;
    }

    public bool IsAdded(WorkItem workItem)
    {
        return _assignedWorkItems.SelectMany(a => a.WorkItems).Any(wi => wi.Id == workItem.Id);
    }

    private ExecutionOrderItem CreateDailyAssignmentForNextDay()
    {
        _currentDay = _currentDay.AddDays(1);

        _logger.LogDebug($"Try to find date for creating daily assignments. Start date: {_currentDay}");
        _currentDay = _workDayChecker.GetNextWork(_currentDay);
        var dailyAssignments = new ExecutionOrderItem(_currentDay, new List<WorkItem>());
        _assignedWorkItems.Add(dailyAssignments);
        return dailyAssignments;
    }
}