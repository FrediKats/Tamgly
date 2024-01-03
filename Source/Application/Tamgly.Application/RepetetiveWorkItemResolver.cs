using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.Projects;
using Tamgly.Core.RepetitiveWorkItems;
using Tamgly.Core.WorkItems;
using Tamgly.DataAccess;
using Tamgly.DataAccess.Models;
using Tamgly.RepetitiveEvents.Models;
using Tamgly.RepetitiveEvents.Tools;

namespace Tamgly.Application;

public class RepetitiveWorkItemResolver : IRepetitiveWorkItemResolver
{
    // TODO: config?
    private readonly int _repetitiveIntervalDays = 7;

    private readonly TamglyDatabaseContext _databaseContext;
    private readonly RepetitiveEventPatternSerializer _repetitiveEventPatternSerializer;
    private readonly OccurrenceGenerator _occurrenceGenerator;

    public RepetitiveWorkItemResolver(TamglyDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _repetitiveEventPatternSerializer = RepetitiveEventPatternSerializer.Instance;
        _occurrenceGenerator = OccurrenceGenerator.Instance;
    }

    public IReadOnlyCollection<WorkItem> CreateWorkItemsFromRepetitive(Project project)
    {
        return CreateWorkItemsFromRepetitive(project.Items.ToList());
    }

    public IReadOnlyCollection<WorkItem> CreateWorkItemsFromRepetitive(IReadOnlyCollection<WorkItem> workItemDatabaseRecords)
    {
        var result = new List<WorkItem>();
        result.AddRange(workItemDatabaseRecords);

        foreach (WorkItem workItem in workItemDatabaseRecords)
            result.AddRange(GetRepetitiveItems(workItem));

        return result;
    }

    public IReadOnlyCollection<WorkItem> GetRepetitiveItems(WorkItem workItem)
    {
        RepetitiveWorkItemConfigurationDatabaseRecord? repetitiveWorkItemConfiguration = _databaseContext.RepetitiveWorkItemConfigurations.Find(workItem.Id);
        if (repetitiveWorkItemConfiguration is null)
            return Array.Empty<WorkItem>();

        IRepetitiveEventPattern? repetitiveEventPattern = _repetitiveEventPatternSerializer.Deserialize(
            repetitiveWorkItemConfiguration.SerializedConfiguration,
            repetitiveWorkItemConfiguration.Type);

        ArgumentNullException.ThrowIfNull(repetitiveEventPattern);
        var fromDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(_repetitiveIntervalDays));

        return CreateForOccurrencesOnInterval(fromDate, endDate, workItem, repetitiveEventPattern);
    }

    private IReadOnlyCollection<WorkItem> CreateForOccurrencesOnInterval(DateOnly from, DateOnly to, WorkItem source, IRepetitiveEventPattern repetitiveEventPattern)
    {
        IReadOnlyCollection<DateOnly> occurrences = _occurrenceGenerator.GetOccurrences(repetitiveEventPattern, from, to);
        return occurrences
            .Select(o => new WorkItem(
                source.Id,
                source.ExternalId,
                source.Title,
                source.Description,
                source.State,
                source.CreationTime,
                source.LastModifiedTime,
                source.CompletedTime,
                source.Estimate,
                WorkItemDeadlineCreator.Create(source.Deadline.DeadlineType, o),
                source.AssignedTo,
                source.Priority))
            .ToList();
    }
}