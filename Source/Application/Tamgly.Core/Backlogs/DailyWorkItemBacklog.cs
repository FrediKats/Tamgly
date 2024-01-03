using System;
using System.Collections.Generic;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Backlogs;

public class DailyWorkItemBacklog
{
    public WorkItemBacklog CurrentDay { get; }
    public WorkItemBacklog CurrentWeek { get; }
    public WorkItemBacklog CurrentMonth { get; }

    public TimeSpan TotalEstimate => CurrentDay.GetTotalEstimates();
    public TimeSpan TotalEstimateForWeek => CurrentWeek.GetTotalEstimates();
    public TimeSpan TotalEstimateForMonth => CurrentMonth.GetTotalEstimates();

    public TimeSpan? WeekEstimatePerDay => CurrentWeek.GetAverageDailyEstimate();
    public TimeSpan? MonthEstimatePerDay => CurrentMonth.GetAverageDailyEstimate();

    private DailyWorkItemBacklog(WorkItemBacklog currentDay, WorkItemBacklog currentWeek, WorkItemBacklog currentMonth)
    {
        ArgumentNullException.ThrowIfNull(currentDay);
        ArgumentNullException.ThrowIfNull(currentWeek);
        ArgumentNullException.ThrowIfNull(currentMonth);

        CurrentDay = currentDay;
        CurrentWeek = currentWeek;
        CurrentMonth = currentMonth;
    }

    public static DailyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        ArgumentNullException.ThrowIfNull(workItems);

        var dailyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyDay(time)), workItems);
        var weeklyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyWeek(time)), workItems);
        var monthlyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyMonth(time)), workItems);
        return new DailyWorkItemBacklog(dailyBacklog, weeklyBacklog, monthlyBacklog);
    }
}