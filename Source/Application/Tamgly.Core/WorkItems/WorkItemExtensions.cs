using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamgly.Core.WorkItems;

public static class WorkItemExtensions
{
    public static DateOnly GetEarliestStart(this IEnumerable<WorkItem> workItems)
    {
        return workItems
            .Select(wi => wi.Deadline.TimeInterval)
            .Where(d => d is not null)
            .Select(d => d!.Start)
            .MinBy(d => d);
    }

    public static DateOnly? GetEarliestEnd(this IEnumerable<WorkItem> workItems)
    {
        var values = workItems
            .Select(wi => wi.Deadline.TimeInterval)
            .Where(d => d is not null)
            .Select(d => d!.End)
            .ToList();

        if (values.Any())
            return values.Max();

        return null;
    }
}