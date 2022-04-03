using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class DailyWorkItemBacklog
{
    public WorkItemBacklog CurrentDay { get; }
    public WorkItemBacklog CurrentWeek { get; }

    public DailyWorkItemBacklog(WorkItemBacklog currentDay, WorkItemBacklog currentWeek)
    {
        CurrentDay = currentDay;
        CurrentWeek = currentWeek;
    }

    public static DailyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateTime time)
    {
        WorkItemBacklog dailyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadline.Type.Day, time), workItems);
        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadline.Type.Week, time), workItems);
        return new DailyWorkItemBacklog(dailyBacklog, weeklyBacklog);
    }
}