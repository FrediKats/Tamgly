using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using System;
using System.Linq;
using NUnit.Framework;
using Tamgly.Application;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.Aggregates;
using Tamgly.Core.Backlogs;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;
using Tamgly.Mapping;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using WorkItem = Tamgly.Core.WorkItems.WorkItem;
using WorkItemBuilder = Tamgly.Core.WorkItems.WorkItemBuilder;

namespace Tamgly.Tests;

public class WorkItemBacklogTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));

    private readonly WorkItemManager _workItemManager;
    private readonly BacklogManager _backlogManager;
    private readonly DateOnly _workItemDeadline;
    private readonly BlockerLinkManager _blockerLinkManager;
    private readonly ILogger _logger;

    public WorkItemBacklogTests()
    {
        _logger = DefaultLoggerConfiguration.CreateConsoleLogger();


        _workItemManager = new WorkItemManager(_logger, new WorkItemTrackingIntervalProvider(TamglyDatabaseContextTestInstanceProvider.Provider(_logger), MappingHolder.Instance));
        _workItemDeadline = FromDateTime;
        _blockerLinkManager = new BlockerLinkManager(_workItemManager, _logger);
        _backlogManager = new BacklogManager(_workItemManager, _logger);

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyWeek(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(8))
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 10", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyWeek(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(12))
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lab 4/5", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyMonth(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(25))
                .Build());
    }

    [Test]
    public void EnsureBacklogCalculateEstimatesCorrect()
    {
        DailyWorkItemBacklog workItemBacklog = _backlogManager.GetDailyBacklog(_workItemDeadline);
        WeeklyWorkItemBacklog weeklyBacklog = _backlogManager.GetWeeklyBacklog(_workItemDeadline);
        MonthlyWorkItemBacklog monthlyBacklog = _backlogManager.GetMonthlyBacklog(_workItemDeadline);

        workItemBacklog.TotalEstimate.Should().Be(TimeSpan.FromHours(3));
        workItemBacklog.TotalEstimateForWeek.Should().Be(TimeSpan.FromHours(20));
        weeklyBacklog.TotalEstimate.Should().Be(TimeSpan.FromHours(20));
        workItemBacklog.TotalEstimateForMonth.Should().Be(TimeSpan.FromHours(25));
        monthlyBacklog.TotalEstimateForMonth.Should().Be(TimeSpan.FromHours(25));
    }

    [Test]
    public void AddBlockLink_EnsureLinkExists()
    {
        WorkItem first = _workItemManager.GetSelfWorkItems().ElementAt(1);
        WorkItem second = _workItemManager.GetSelfWorkItems().ElementAt(2);
        _blockerLinkManager.AddLink(first.Id, second.Id);
        _blockerLinkManager.IsBlocked(second).Should().BeTrue();
    }

    [Test]
    public void AddWorkItemToNotMe_EnsureCountIsValid()
    {
        int allWorkItemsCount = _workItemManager.GetAllWorkItems().Count;
        int myWorkItemsCount = _workItemManager.GetSelfWorkItems().Count;

        myWorkItemsCount.Should().Be(allWorkItemsCount);

        var myNewJunior = new Person(Guid.NewGuid(), "Mr. Junior");
        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetAssigning(myNewJunior)
                .Build());

        int newAllWorkItemsCount = _workItemManager.GetAllWorkItems().Count;
        int newMyWorkItemsCount = _workItemManager.GetSelfWorkItems().Count;

        myWorkItemsCount.Should().Be(newMyWorkItemsCount);
        newAllWorkItemsCount.Should().Be(allWorkItemsCount + 1);
    }

    [Test]
    public void AddBlockLink_CorrectTotalPriority()
    {
        WorkItem first = _workItemManager.GetSelfWorkItems().ElementAt(0);
        WorkItem second = _workItemManager.GetSelfWorkItems().ElementAt(1);
        _blockerLinkManager.AddLink(first.Id, second.Id);

        _blockerLinkManager.CalculateTotalPriority(first).Should().Be(WorkItemPriority.P1);
    }

    [Test]
    public void AddBlockLink_AffectBacklogListOrder()
    {
        DailyWorkItemBacklog workItemBacklog = _backlogManager.GetDailyBacklog(_workItemDeadline);

        WorkItem first = workItemBacklog.CurrentDay.Items.ElementAt(0);
        WorkItem second = workItemBacklog.CurrentDay.Items.ElementAt(1);
        Assert.That(first.Priority, Is.GreaterThan(second.Priority));

        //_blockerLinkManager.AddLink(first.Id, second.Id);

        var backlogManager = new BacklogManager(new PrioritizedWorkItemManager(_workItemManager, _blockerLinkManager), _logger);
        workItemBacklog = backlogManager.GetDailyBacklog(_workItemDeadline);
        first = workItemBacklog.CurrentDay.Items.ElementAt(0);
        second = workItemBacklog.CurrentDay.Items.ElementAt(1);
        Assert.That(first.Priority, Is.LessThan(second.Priority));
    }
}