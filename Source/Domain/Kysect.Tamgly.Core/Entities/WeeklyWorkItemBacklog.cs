using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WeeklyWorkItemBacklog
{
    public WorkItemBacklog CurrentWeek { get; }

    public WeeklyWorkItemBacklog(WorkItemBacklog currentWeek)
    {
        ArgumentNullException.ThrowIfNull(currentWeek);

        CurrentWeek = currentWeek;
    }

    public static WeeklyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        ArgumentNullException.ThrowIfNull(workItems);

        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Week, time), workItems);
        return new WeeklyWorkItemBacklog(weeklyBacklog);
    }
}