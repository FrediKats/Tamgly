﻿using Kysect.Tamgly.Core.Entities;

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
        return DailyWorkItemBacklog.Create(_itemManager.GetWorkItems(), time);
    }

    public WeeklyWorkItemBacklog GetWeeklyBacklog(DateOnly time)
    {
        return WeeklyWorkItemBacklog.Create(_itemManager.GetWorkItems(), time);
    }
}