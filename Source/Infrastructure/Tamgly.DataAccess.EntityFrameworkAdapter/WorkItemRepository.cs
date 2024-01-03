using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.DataAccess.Models;
using Tamgly.DataAccess.Repositories;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly TamglyEntityFrameworkDbContext _context;

    public WorkItemRepository(TamglyEntityFrameworkDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<WorkItemDatabaseRecord> GetWorkItems()
    {
        return _context.WorkItems.ToList();
    }

    public WorkItemDatabaseRecord? FindByExternalId(string externalId)
    {
        return _context
            .WorkItems
            .FirstOrDefault(w => w.ExternalId == externalId);
    }

    public IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> GetWorkItemWithProjectAssociations()
    {
        var projects = _context.Projects.ToDictionary(p => p.Id, p => p);
        var workItemToProjectMapping = _context.ProjectWorkItems.ToDictionary(p => p.WorkItemId, p => p.ProjectId);
        var workItems = _context.WorkItems.ToList();

        return workItems
            .Select(wi =>
            {
                ProjectDatabaseRecord? project = workItemToProjectMapping.TryGetValue(wi.Id, out Guid projectId)
                    ? projects[projectId]
                    : null;

                return new WorkItemWithProjectAssociationDatabaseRecord(wi, project);
            })
            .ToList();
    }

    public void AddWorkItems(IReadOnlyCollection<WorkItemDatabaseRecord> workItems)
    {
        _context.WorkItems.AddRange(workItems);
        _context.SaveChanges();
    }

    public void UpdateWorkItems(IReadOnlyCollection<WorkItemDatabaseRecord> workItems)
    {
        // TODO: implement
        //throw new NotImplementedException();
    }
}