﻿using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.Backlogs;

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

        WorkItemBacklog dailyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Day, time), workItems);
        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Week, time), workItems);
        WorkItemBacklog monthlyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Month, time), workItems);
        return new DailyWorkItemBacklog(dailyBacklog, weeklyBacklog, monthlyBacklog);
    }
}