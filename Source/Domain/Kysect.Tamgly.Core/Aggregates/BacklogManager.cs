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

    public DailyBacklog GetDailyBacklog(DateTime time)
    {
        List<WorkItem> workItems = _itemManager
            .GetWorkItems()
            .Where(i => i.Deadline == time)
            .ToList();

        return new DailyBacklog(time, workItems);
    }

    public WeeklyBacklog GetWeeklyBacklog(DateTime time)
    {
        TamglyWeek tamglyWeek = TamglyWeek.FromDate(time);
        List<WorkItem> workItems = _itemManager
            .GetWorkItems()
            .Where(i => i.Deadline is not null && tamglyWeek.Contains(i.Deadline.Value))
            .ToList();

        return new WeeklyBacklog(tamglyWeek, workItems);
    }
}