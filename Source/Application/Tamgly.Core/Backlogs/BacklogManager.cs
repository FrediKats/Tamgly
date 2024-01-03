using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Tamgly.Core.Aggregates;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Backlogs;

public class BacklogManager
{
    private readonly IWorkItemManager _itemManager;
    private readonly ILogger _logger;

    public BacklogManager(IWorkItemManager itemManager, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(itemManager);

        _itemManager = itemManager;
        _logger = logger;
    }

    public DailyWorkItemBacklog GetDailyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        _logger.LogDebug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return DailyWorkItemBacklog.Create(workItems, time);
    }

    public WeeklyWorkItemBacklog GetWeeklyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        _logger.LogDebug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return WeeklyWorkItemBacklog.Create(workItems, time);
    }

    public MonthlyWorkItemBacklog GetMonthlyBacklog(DateOnly time)
    {
        IReadOnlyCollection<WorkItem> workItems = _itemManager.GetSelfWorkItems();

        _logger.LogDebug($"Create daily backlog for {time}. Work items count: {workItems.Count}");

        return MonthlyWorkItemBacklog.Create(workItems, time);
    }
}