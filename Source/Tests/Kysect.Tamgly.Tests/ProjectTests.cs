using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.ValueObjects;
using Serilog;
using System;
using System.Collections.Generic;
using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Tools;
using NUnit.Framework;

namespace Kysect.Tamgly.Tests;

public class ProjectTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));


    private readonly WorkItemManager _workItemManager;
    private readonly BacklogManager _backlogManager;
    private readonly DateOnly _workItemDeadline;
    private readonly BlockerLinkManager _blockerLinkManager;

    public ProjectTests()
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

    }

    [Test]
    public void AddToMuchTaskToProject_ShouldReturnWarnings()
    {
        var project = Project.Create("Test project", new WorkingHours(TimeSpan.FromHours(1), null, null, null));
        _workItemManager.AddProject(project);

        WorkItem workItem1 = new WorkItemBuilder("WI1")
            .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
            .SetPriority(WorkItemPriority.P3)
            .SetEstimates(TimeSpan.FromHours(1))
            .Build();
        _workItemManager.AddWorkItem(workItem1, project);

        WorkItem workItem2 = new WorkItemBuilder("WI2")
            .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
            .SetPriority(WorkItemPriority.P3)
            .SetEstimates(TimeSpan.FromHours(1))
            .Build();
        _workItemManager.AddWorkItem(workItem2, project);

        IReadOnlyCollection<WorkingHoursWarning> workingHoursWarnings = new ProjectWorkHoursValidator().Validate(project);

        Assert.AreEqual(1, workingHoursWarnings.Count);
    }
}