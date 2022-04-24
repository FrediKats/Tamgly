using System;
using System.Linq;
using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Entities.Backlogs;
using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;
using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;
using NUnit.Framework;

namespace Kysect.Tamgly.Tests;

public class WorkItemBacklogTests
{
    private readonly BacklogManager _backlogManager;
    private readonly DateOnly _workItemDeadline;
    private readonly WorkItemManager _workItemManager;
    private readonly BlockerLinkManager _blockerLinkManager;

    public WorkItemBacklogTests()
    {
        _workItemManager = new WorkItemManager();
        _backlogManager = new BacklogManager(_workItemManager);
        _workItemDeadline = DateOnly.FromDateTime(new DateTime(2022, 04, 08));
        _blockerLinkManager = new BlockerLinkManager(_workItemManager);

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .Build());

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Lecture 09")
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetEstimates(TimeSpan.FromHours(3))
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

        var backlogManager = new BacklogManager(_workItemManager);
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
        IWorkItem first = _workItemManager.GetSelfWorkItems().ElementAt(1);
        IWorkItem second = _workItemManager.GetSelfWorkItems().ElementAt(2);
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
}