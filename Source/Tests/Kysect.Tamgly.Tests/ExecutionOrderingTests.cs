using Kysect.Tamgly.Core;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Tests;

public class ExecutionOrderingTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));

    public ExecutionOrderingTests()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    [Test]
    public void AddToMuchTaskToProject_ShouldReturnWarnings()
    {
        var executionOrderManager = new ExecutionOrderManager(FromDateTime);

        var workItemManager = new WorkItemManager();
        var workItemDeadline = FromDateTime;

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly")
                .SetDeadline(new WorkItemDeadline(new TamglyWeek(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(2))
                .Build());

        IReadOnlyCollection<WorkItem> workItems = workItemManager.GetAllWorkItems();

        ExecutionOrder executionOrder = executionOrderManager.Order(workItems, SelectedDayOfWeek.All, TimeSpan.FromHours(5));

        Assert.AreEqual(2, executionOrder.Items.Count);
    }
}