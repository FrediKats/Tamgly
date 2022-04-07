using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WeeklyWorkItemBacklog
{
    public WorkItemBacklog CurrentWeek { get; }

    public WeeklyWorkItemBacklog(WorkItemBacklog currentWeek)
    {
        CurrentWeek = currentWeek;
    }

    public static WeeklyWorkItemBacklog Create(IReadOnlyCollection<WorkItem> workItems, DateOnly time)
    {
        WorkItemBacklog weeklyBacklog = WorkItemBacklog.Create(WorkItemDeadline.Create(WorkItemDeadline.Type.Week, time), workItems);
        return new WeeklyWorkItemBacklog(weeklyBacklog);
    }
}