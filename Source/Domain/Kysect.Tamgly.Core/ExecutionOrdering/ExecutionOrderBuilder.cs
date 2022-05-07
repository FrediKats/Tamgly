using Kysect.Tamgly.Common;
using Serilog;

namespace Kysect.Tamgly.Core;

public class ExecutionOrderBuilder
{
    private DateOnly _currentDay;

    private readonly SelectedDayOfWeek _selectedDayOfWeek;
    private readonly TimeSpan _workingHoursPerDay;
    private readonly List<ExecutionOrderItem> _assignedWorkItems;

    public ExecutionOrderBuilder(DateOnly currentDay, SelectedDayOfWeek selectedDayOfWeek, TimeSpan workingHoursPerDay)
    {
        if (selectedDayOfWeek == SelectedDayOfWeek.None)
            throw new TamglyException($"Cannot create Daily assignments because no work day selected");

        _currentDay = currentDay;
        _selectedDayOfWeek = selectedDayOfWeek;
        _workingHoursPerDay = workingHoursPerDay;
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
            Log.Debug($"No daily assignments exists. Create new for date: {_currentDay}");
            return CreateNewDailyAssignment(_currentDay);
        }

        ExecutionOrderItem lastAssignment = _assignedWorkItems.Last();
        if (lastAssignment.TotalEstimates() + estimates > _workingHoursPerDay)
        {
            Log.Debug($"Cannot add WI with estimate {estimates} to daily assignments {lastAssignment.Date}. Not enough free time");
            _currentDay = _currentDay.AddDays(1);
            return CreateNewDailyAssignment(_currentDay);
        }

        return lastAssignment;
    }

    public bool IsAdded(WorkItem workItem)
    {
        return _assignedWorkItems.SelectMany(a => a.WorkItems).Any(wi => wi.Id == workItem.Id);
    }

    private ExecutionOrderItem CreateNewDailyAssignment(DateOnly searchFrom)
    {
        Log.Debug($"Try to find date for creating daily assignments. Start date: {searchFrom}, ");
        _currentDay = searchFrom.NextDayInRange(_selectedDayOfWeek);
        var dailyAssignments = new ExecutionOrderItem(_currentDay, new List<WorkItem>());
        _assignedWorkItems.Add(dailyAssignments);
        return dailyAssignments;
    }
}