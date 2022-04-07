using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.Backlogs;

public class WeeklyWorkItemBacklog
{
    public WorkItemBacklog CurrentWeek { get; }
    public WorkItemBacklog CurrentMonth { get; }

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

        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Week, time), workItems);
        WorkItemBacklog monthlyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Month, time), workItems);
        return new WeeklyWorkItemBacklog(weeklyBacklog, monthlyBacklog);
    }
}