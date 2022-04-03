using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Aggregates;

public class BacklogManager
{
    private readonly WorkItemManager _itemManager;

    public BacklogManager(WorkItemManager itemManager)
    {
        _itemManager = itemManager;
    }

    public WorkItemBacklog GetDailyBacklog(DateTime time)
    {
        var workItemDeadline = WorkItemDeadline.Create(WorkItemDeadline.Type.Day, time);
        return CreateBacklog(workItemDeadline);
    }

    public WorkItemBacklog GetWeeklyBacklog(DateTime time)
    {
        var workItemDeadline = WorkItemDeadline.Create(WorkItemDeadline.Type.Week, time);
        return CreateBacklog(workItemDeadline);
    }

    private WorkItemBacklog CreateBacklog(WorkItemDeadline deadline)
    {
        List<WorkItem> workItems = _itemManager
            .GetWorkItems()
            .Where(i => i.Deadline.MatchedWith(deadline))
            .ToList();

        return new WorkItemBacklog(deadline, workItems);
    }
}