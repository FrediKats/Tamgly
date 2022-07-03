namespace Kysect.Tamgly.Core;

public class MonthlyWorkItemBacklog
{
    public WorkItemBacklog CurrentMonth { get; }

    public TimeSpan TotalEstimateForMonth => CurrentMonth.GetTotalEstimates();

    public TimeSpan? MonthEstimatePerDay => CurrentMonth.GetAverageDailyEstimate();

    public MonthlyWorkItemBacklog(WorkItemBacklog currentMonth)
    {
        ArgumentNullException.ThrowIfNull(currentMonth);

        CurrentMonth = currentMonth;
    }

    public static MonthlyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        ArgumentNullException.ThrowIfNull(workItems);

        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(new WorkItemDeadline(new TamglyMonth(time)), workItems);
        return new MonthlyWorkItemBacklog(weeklyBacklog);
    }
}