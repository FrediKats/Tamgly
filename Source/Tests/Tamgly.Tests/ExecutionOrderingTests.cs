using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tamgly.Core.ExecutionOrdering.Models;
using Tamgly.Application;
using Tamgly.Core.Aggregates;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.ExecutionOrdering;
using Tamgly.Core.RepetitiveWorkItems;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;
using Tamgly.Mapping;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using WorkItem = Tamgly.Core.WorkItems.WorkItem;
using WorkItemBuilder = Tamgly.Core.WorkItems.WorkItemBuilder;
using Kysect.CommonLib.DateAndTime;
using Kysect.CommonLib.DependencyInjection.Logging;
using Tamgly.Common.IdentifierGenerators;

namespace Tamgly.Tests;

public class EmptyRepetitiveWorkItemResolver : IRepetitiveWorkItemResolver
{
    public static EmptyRepetitiveWorkItemResolver Instance { get; } = new EmptyRepetitiveWorkItemResolver();

    public IReadOnlyCollection<WorkItem> GetRepetitiveItems(WorkItem workItem)
    {
        return Array.Empty<WorkItem>();
    }
}

public class ExecutionOrderingTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));
    private readonly ILogger _logger;

    public ExecutionOrderingTests()
    {
        _logger = DefaultLoggerConfiguration.CreateConsoleLogger();
    }

    [Test]
    public void CreateExecutionOrder_ShouldReturnCorrectCountOfDays()
    {
        var executionOrderManager = new ExecutionOrderManager(FromDateTime, SelectedDayOfWeek.All, TimeSpan.FromHours(5), _logger);

        var workItemManager = new WorkItemManager(_logger, new WorkItemTrackingIntervalProvider(TamglyDatabaseContextTestInstanceProvider.Provider(_logger), MappingHolder.Instance));
        DateOnly workItemDeadline = FromDateTime;

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(2))
                .SetPriority(WorkItemPriority.P2)
                .Build());

        IReadOnlyCollection<WorkItem> workItems = workItemManager.GetAllWorkItems();

        ExecutionOrder executionOrder = executionOrderManager.Order(workItems);

        executionOrder.Items.Count(eoi => eoi.WorkItems.Any()).Should().Be(2);
    }

    [Test]
    public void PredictExecutionOrder_ShouldPredictNextDay()
    {
        var executionOrderManager = new ExecutionOrderManager(FromDateTime, SelectedDayOfWeek.All, TimeSpan.FromHours(5), _logger);

        var workItemManager = new WorkItemManager(_logger, new WorkItemTrackingIntervalProvider(TamglyDatabaseContextTestInstanceProvider.Provider(_logger), MappingHolder.Instance));
        DateOnly workItemDeadline = FromDateTime;

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(2))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        IReadOnlyCollection<WorkItem> workItems = workItemManager.GetAllWorkItems();
        ExecutionOrder executionOrder = executionOrderManager.Order(workItems);

        WorkItem newWorkItem = new WorkItemBuilder("New WI without predicted time", InMemoryIdentifierGenerator.Instance)
            .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
            .SetEstimates(TimeSpan.FromHours(4))
            .SetPriority(WorkItemPriority.P4)
            .Build();
        ExecutionOrderItem placeForNewWorkItem = executionOrder.GetPlaceForNewWorkItem(newWorkItem, TimeSpan.FromHours(5), SelectedDayOfWeek.All);

        placeForNewWorkItem.Date.Should().Be(FromDateTime.AddDays(3));
    }

    [Test]
    public void GetExecutionOrderDiff_EnsureThreeElementsChanged()
    {
        var executionOrderManager = new ExecutionOrderManager(FromDateTime, SelectedDayOfWeek.All, TimeSpan.FromHours(5), _logger);

        var workItemManager = new WorkItemManager(_logger, new WorkItemTrackingIntervalProvider(TamglyDatabaseContextTestInstanceProvider.Provider(_logger), MappingHolder.Instance));
        DateOnly workItemDeadline = FromDateTime;

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P2)
                .Build());

        workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(2))
                .SetPriority(WorkItemPriority.P2)
                .Build());

        IReadOnlyCollection<WorkItem> workItems = workItemManager.GetAllWorkItems();

        WorkItem newWorkItem = new WorkItemBuilder("New WI without predicted time", InMemoryIdentifierGenerator.Instance)
            .SetDeadline(new WorkItemDeadline(new TamglyDay(workItemDeadline)))
            .SetEstimates(TimeSpan.FromHours(4))
            .SetPriority(WorkItemPriority.P1)
            .Build();

        IReadOnlyCollection<ExecutionOrderDiff> diffAfterAddingWorkItem = executionOrderManager.GetDiffAfterAddingWorkItem(workItems, newWorkItem);

        diffAfterAddingWorkItem.Should().HaveCount(3);
    }
}