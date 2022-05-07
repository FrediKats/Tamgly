using System;
using System.Linq;
using Kysect.Tamgly.Core;
using NUnit.Framework;
using Serilog;

namespace Kysect.Tamgly.Tests;

public class WorkItemBacklogTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));
    
    private readonly WorkItemManager _workItemManager;
    private readonly BacklogManager _backlogManager;
    private readonly DateOnly _workItemDeadline;
    private readonly BlockerLinkManager _blockerLinkManager;

    public WorkItemBacklogTests()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        _workItemManager = new WorkItemManager();
        _workItemDeadline = FromDateTime;
        _blockerLinkManager = new BlockerLinkManager(_workItemManager);
        _backlogManager = new BacklogManager(_workItemManager);

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetPriority(WorkItemPriority.P3)
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
                .SetPriority(WorkItemPriority.P1)
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Tamgly")
                .SetDeadline(new WorkItemDeadline(new TamglyWeek(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(8))
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 10")
                .SetDeadline(new WorkItemDeadline(new TamglyWeek(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(12))
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lab 4/5")
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

        Assert.AreEqual(TimeSpan.FromHours(3), workItemBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(20), workItemBacklog.TotalEstimateForWeek);
        Assert.AreEqual(TimeSpan.FromHours(20), weeklyBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(25), workItemBacklog.TotalEstimateForMonth);
        Assert.AreEqual(TimeSpan.FromHours(25), monthlyBacklog.TotalEstimateForMonth);
    }

    [Test]
    public void WithRepetitiveTask()
    {
        _workItemManager.AddWorkItem(
            new RepetitiveParentWorkItemBuilder("Daily", new DailyPeriodicRepetitiveInterval(new TamglyWeek(_workItemDeadline), 1))
                .SetEstimates(TimeSpan.FromHours(1))
                .Build());

        var backlogManager = new BacklogManager(new PrioritizedWorkItemManager(_workItemManager, _blockerLinkManager));
        DailyWorkItemBacklog workItemBacklog = backlogManager.GetDailyBacklog(_workItemDeadline);
        DailyWorkItemBacklog tomorrowWorkItemBacklog = backlogManager.GetDailyBacklog(_workItemDeadline.AddDays(1));
        WeeklyWorkItemBacklog weeklyBacklog = backlogManager.GetWeeklyBacklog(_workItemDeadline);
        MonthlyWorkItemBacklog monthlyBacklog = backlogManager.GetMonthlyBacklog(_workItemDeadline);

        Assert.AreEqual(TimeSpan.FromHours(4), workItemBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(1), tomorrowWorkItemBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(20), workItemBacklog.TotalEstimateForWeek);
        Assert.AreEqual(TimeSpan.FromHours(20), weeklyBacklog.TotalEstimate);
        Assert.AreEqual(TimeSpan.FromHours(25), workItemBacklog.TotalEstimateForMonth);
        Assert.AreEqual(TimeSpan.FromHours(25), monthlyBacklog.TotalEstimateForMonth);
    }

    [Test]
    public void AddBlockLink_EnsureLinkExists()
    {
        WorkItem first = _workItemManager.GetSelfWorkItems().ElementAt(1);
        WorkItem second = _workItemManager.GetSelfWorkItems().ElementAt(2);
        _blockerLinkManager.AddLink(first.Id, second.Id);
        Assert.AreEqual(true, _blockerLinkManager.IsBlocked(second));
    }

    [Test]
    public void AddWorkItemToNotMe_EnsureCountIsValid()
    {
        int allWorkItemsCount = _workItemManager.GetAllWorkItems().Count;
        int myWorkItemsCount = _workItemManager.GetSelfWorkItems().Count;

        Assert.AreEqual(allWorkItemsCount, myWorkItemsCount);

        Person myNewJunior = new Person(Guid.NewGuid(), "Mr. Junior");
        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetAssigning(myNewJunior)
                .Build());

        int newAllWorkItemsCount = _workItemManager.GetAllWorkItems().Count;
        int newMyWorkItemsCount = _workItemManager.GetSelfWorkItems().Count;

        Assert.AreEqual(myWorkItemsCount, newMyWorkItemsCount);
        Assert.AreEqual(allWorkItemsCount + 1, newAllWorkItemsCount);
    }

    [Test]
    public void AddBlockLink_CorrectTotalPriority()
    {
        WorkItem first = _workItemManager.GetSelfWorkItems().ElementAt(0);
        WorkItem second = _workItemManager.GetSelfWorkItems().ElementAt(1);
        _blockerLinkManager.AddLink(first.Id, second.Id);
        Assert.AreEqual(WorkItemPriority.P1, _blockerLinkManager.CalculateTotalPriority(first));
    }

    [Test]
    public void AddBlockLink_AffectBacklogListOrder()
    {
        DailyWorkItemBacklog workItemBacklog = _backlogManager.GetDailyBacklog(_workItemDeadline);
        
        WorkItem first = workItemBacklog.CurrentDay.Items.ElementAt(0);
        WorkItem second = workItemBacklog.CurrentDay.Items.ElementAt(1);
        Assert.Greater(first.Priority, second.Priority);

        //_blockerLinkManager.AddLink(first.Id, second.Id);
        
        var backlogManager = new BacklogManager(new PrioritizedWorkItemManager(_workItemManager, _blockerLinkManager));
        workItemBacklog = backlogManager.GetDailyBacklog(_workItemDeadline);
        first = workItemBacklog.CurrentDay.Items.ElementAt(0);
        second = workItemBacklog.CurrentDay.Items.ElementAt(1);
        Assert.Less(first.Priority, second.Priority);
    }
}