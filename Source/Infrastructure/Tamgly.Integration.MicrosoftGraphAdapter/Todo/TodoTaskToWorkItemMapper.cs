using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.Graph.Models;
using System;
using System.Linq;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.WorkItems;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo;

public class TodoTaskToWorkItemMapper
{
    private readonly IIdentifierGenerator _identifierGenerator;

    public TodoTaskToWorkItemMapper(IIdentifierGenerator identifierGenerator)
    {
        _identifierGenerator = identifierGenerator;
    }

    public WorkItem Convert(TodoTask task)
    {
        WorkItemState state = task.Status == TaskStatus.Completed
            ? WorkItemState.Closed
            : WorkItemState.Open;

        task.Id.ThrowIfNull(nameof(task.Id));
        task.Title.ThrowIfNull(nameof(task.Title));

        DateTimeOffset creationTime = task.CreatedDateTime ?? DateTimeOffset.MinValue;;
        DateTimeOffset lastModifiedTime = task.LastModifiedDateTime?.UtcDateTime ?? DateTimeOffset.MinValue;
        WorkItem workItem = new WorkItemBuilder(task.Title, _identifierGenerator)
            .SetExternalId(MicrosoftTodoExternalId.For(task.Id).Value)
            .SetDescription(task.Body?.Content)
            .SetState(state)
            .SetCreationTime(creationTime)
            .SetLastModifiedTime(lastModifiedTime)
            // TODO: implement parsing Completing
            .SetCompletedTime(null)
            .SetEstimates(CalculateEstimates(task))
            .SetPriority(FindPriority(task))
            .Build();

        return workItem;
    }

    private TimeSpan? CalculateEstimates(TodoTask task)
    {
        var categories = task.Categories
            .Where(c => c.StartsWith("Time: "))
            .ToList();
        if (!categories.Any())
            return null;

        double estimates = categories
            .Select(s => s.Substring(6))
            .Select(double.Parse)
            .Sum();

        return TimeSpan.FromHours(estimates);
    }

    private WorkItemPriority? FindPriority(TodoTask task)
    {
        var categories = task.Categories.ToList();
        if (categories.Contains("P1"))
            return WorkItemPriority.P1;

        if (categories.Contains("P2"))
            return WorkItemPriority.P2;

        if (categories.Contains("P3"))
            return WorkItemPriority.P3;

        if (categories.Contains("P4"))
            return WorkItemPriority.P4;

        if (categories.Contains("P5"))
            return WorkItemPriority.P5;

        return null;
    }
}