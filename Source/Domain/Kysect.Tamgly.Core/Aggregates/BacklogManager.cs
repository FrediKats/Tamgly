using Kysect.Tamgly.Core.Entities;

namespace Kysect.Tamgly.Core.Aggregates;

public class BacklogManager
{
    private readonly WorkItemManager _itemManager;

    public BacklogManager(WorkItemManager itemManager)
    {
        _itemManager = itemManager;
    }

    public DailyBacklog GetDailyBacklog(DateOnly date)
    {
        List<WorkItem> workItems = _itemManager
            .GetWorkItems()
            .Where(i => i.Deadline == date)
            .ToList();

        return new DailyBacklog(date, workItems);
    }
}