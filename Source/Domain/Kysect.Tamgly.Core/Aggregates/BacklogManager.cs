using Kysect.Tamgly.Core.Entities.Backlogs;

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
        return DailyWorkItemBacklog.Create(_itemManager.GetSelfWorkItems(), time);
    }

    public WeeklyWorkItemBacklog GetWeeklyBacklog(DateOnly time)
    {
        return WeeklyWorkItemBacklog.Create(_itemManager.GetSelfWorkItems(), time);
    }

    public MonthlyWorkItemBacklog GetMonthlyBacklog(DateOnly time)
    {
        return MonthlyWorkItemBacklog.Create(_itemManager.GetSelfWorkItems(), time);
    }
}