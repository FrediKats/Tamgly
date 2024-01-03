using System.Collections.Generic;
using Tamgly.DataAccess.Models;

namespace Tamgly.DataAccess.Repositories;

public interface IWorkItemRepository
{
    IReadOnlyCollection<WorkItemDatabaseRecord> GetWorkItems();
    WorkItemDatabaseRecord? FindByExternalId(string externalId);
    IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> GetWorkItemWithProjectAssociations();
    void AddWorkItems(IReadOnlyCollection<WorkItemDatabaseRecord> workItems);
    void UpdateWorkItems(IReadOnlyCollection<WorkItemDatabaseRecord> workItems);
}

public static class WorkItemRepositoryExtensions
{
    public static void AddWorkItem(this IWorkItemRepository repository, WorkItemDatabaseRecord workItem)
    {
        repository.AddWorkItems(new[] { workItem });
    }
}