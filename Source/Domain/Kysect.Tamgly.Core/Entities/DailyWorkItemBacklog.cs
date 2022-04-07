using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class DailyWorkItemBacklog
{
    public WorkItemBacklog CurrentDay { get; }
    public WorkItemBacklog CurrentWeek { get; }

    public DailyWorkItemBacklog(WorkItemBacklog currentDay, WorkItemBacklog currentWeek)
    {
        ArgumentNullException.ThrowIfNull(currentDay);
        ArgumentNullException.ThrowIfNull(currentWeek);

        CurrentDay = currentDay;
        CurrentWeek = currentWeek;
    }

    public static DailyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        ArgumentNullException.ThrowIfNull(workItems);

        WorkItemBacklog dailyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Day, time), workItems);
        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadlineType.Week, time), workItems);
        return new DailyWorkItemBacklog(dailyBacklog, weeklyBacklog);
    }
}