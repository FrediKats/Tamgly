using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tamgly.Application;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.Aggregates;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.Projects;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;
using Tamgly.Mapping;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using WorkItem = Tamgly.Core.WorkItems.WorkItem;
using WorkItemBuilder = Tamgly.Core.WorkItems.WorkItemBuilder;

namespace Tamgly.Tests;

public class ProjectTests
{
    private static readonly DateOnly FromDateTime = DateOnly.FromDateTime(new DateTime(2022, 04, 08));

    private readonly WorkItemManager _workItemManager;
    private readonly DateOnly _workItemDeadline;
    private readonly ILogger _logger;

    public ProjectTests()
    {
        _logger = DefaultLoggerConfiguration.CreateConsoleLogger();

        _workItemManager = new WorkItemManager(_logger, new WorkItemTrackingIntervalProvider(TamglyDatabaseContextTestInstanceProvider.Provider(_logger), MappingHolder.Instance));
        _workItemDeadline = FromDateTime;

        _workItemManager.AddWorkItem(
            new WorkItemBuilder("Courses", InMemoryIdentifierGenerator.Instance)
                .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
                .SetPriority(WorkItemPriority.P3)
                .Build());
    }

    [Test]
    public void AddToMuchTaskToProject_ShouldReturnWarnings()
    {
        var project = Project.Create("Test project", new WorkingHours(TimeSpan.FromHours(1), null, null, null));
        _workItemManager.AddProject(project);

        WorkItem workItem1 = new WorkItemBuilder("WI1", InMemoryIdentifierGenerator.Instance)
            .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
            .SetPriority(WorkItemPriority.P3)
            .SetEstimates(TimeSpan.FromHours(1))
            .Build();
        _workItemManager.AddWorkItem(workItem1, project);

        WorkItem workItem2 = new WorkItemBuilder("WI2", InMemoryIdentifierGenerator.Instance)
            .SetDeadline(new WorkItemDeadline(new TamglyDay(_workItemDeadline)))
            .SetPriority(WorkItemPriority.P3)
            .SetEstimates(TimeSpan.FromHours(1))
            .Build();
        _workItemManager.AddWorkItem(workItem2, project);

        IReadOnlyCollection<WorkingHoursWarning> workingHoursWarnings = new ProjectWorkHoursValidator().Validate(project, EmptyRepetitiveWorkItemResolver.Instance);

        workingHoursWarnings.Should().HaveCount(1);
    }
}