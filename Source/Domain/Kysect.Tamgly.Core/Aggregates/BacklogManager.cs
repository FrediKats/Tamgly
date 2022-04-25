using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Entities.Backlogs;
using Serilog;

namespace Kysect.Tamgly.Core.Aggregates;

public class BacklogManager
{
    private readonly WorkItemManager _itemManager;

    public BacklogManager(WorkItemManager itemManager)
    {
        ArgumentNullException.ThrowIfNull(itemManager);

        _itemManager = itemManager;
    }

    public DailyWorkItemBacklog GetDailyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();
        
        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return DailyWorkItemBacklog.Create(workItems, time);
    }

    public WeeklyWorkItemBacklog GetWeeklyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return WeeklyWorkItemBacklog.Create(workItems, time);
    }

    public MonthlyWorkItemBacklog GetMonthlyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        Log.Debug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return MonthlyWorkItemBacklog.Create(workItems, time);
    }
}