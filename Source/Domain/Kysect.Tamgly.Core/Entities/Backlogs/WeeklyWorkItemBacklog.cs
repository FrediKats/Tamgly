using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.TimeIntervals;

namespace Kysect.Tamgly.Core.Entities.Backlogs;

public class WeeklyWorkItemBacklog
{
    public WorkItemBacklog CurrentWeek { get; }
    public WorkItemBacklog CurrentMonth { get; }

    public TimeSpan TotalEstimate => CurrentWeek.GetTotalEstimates();
    public TimeSpan TotalEstimateForMonth => CurrentMonth.GetTotalEstimates();

    public TimeSpan? WeekEstimatePerDay => CurrentWeek.GetAverageDailyEstimate();
    public TimeSpan? MonthEstimatePerDay => CurrentMonth.GetAverageDailyEstimate();

    public WeeklyWorkItemBacklog(WorkItemBacklog currentWeek, WorkItemBacklog currentMonth)
    {
        ArgumentNullException.ThrowIfNull(currentWeek);
        ArgumentNullException.ThrowIfNull(currentMonth);

        CurrentWeek = currentWeek;
        CurrentMonth = currentMonth;
    }

    public static WeeklyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        ArgumentNullException.ThrowIfNull(workItems);

        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyWeek(time)), workItems);
        WorkItemBacklog monthlyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyMonth(time)), workItems);
        return new WeeklyWorkItemBacklog(weeklyBacklog, monthlyBacklog);
    }
}