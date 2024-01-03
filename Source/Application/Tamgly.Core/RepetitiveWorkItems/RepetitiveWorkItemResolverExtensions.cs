using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.Projects;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.RepetitiveWorkItems;

public static class RepetitiveWorkItemResolverExtensions
{
    public static IReadOnlyCollection<WorkItem> CreateWorkItemsFromRepetitive(this IRepetitiveWorkItemResolver resolver, Project project)
    {
        return CreateWorkItemsFromRepetitive(resolver, project.Items.ToList());
    }

    public static IReadOnlyCollection<WorkItem> CreateWorkItemsFromRepetitive(this IRepetitiveWorkItemResolver resolver, IReadOnlyCollection<WorkItem> workItemDatabaseRecords)
    {
        var result = new List<WorkItem>();
        result.AddRange(workItemDatabaseRecords);

        foreach (WorkItem workItem in workItemDatabaseRecords)
            result.AddRange(resolver.GetRepetitiveItems(workItem));

        return result;
    }
}