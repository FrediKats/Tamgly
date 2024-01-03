using Tamgly.DataAccess.Models;
using Tamgly.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public class ProjectRepository : IProjectRepository
{
    private readonly TamglyEntityFrameworkDbContext _context;
    private readonly ILogger _logger;

    public ProjectRepository(TamglyEntityFrameworkDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public IReadOnlyCollection<ProjectDatabaseRecord> GetProjects()
    {
        return _context
            .Projects
            .ToList();
    }

    public ProjectDatabaseRecord GetOrCreateProject(string title)
    {
        ProjectDatabaseRecord? project = _context
            .Projects
            .FirstOrDefault(p => p.Title == title);

        if (project == null)
        {
            _logger.LogInformation($"Project with name {title} does not exists. Will create new project.");

            EntityEntry<ProjectDatabaseRecord> entityEntry = _context.Projects.Add(new ProjectDatabaseRecord { Title = title });
            _context.SaveChanges();
            project = entityEntry.Entity;

            _logger.LogInformation($"Project {title} created. Id: {project.Id}.");
        }

        return project;
    }

    public void CreateWorkItems(IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> workItems)
    {
        foreach (WorkItemWithProjectAssociationDatabaseRecord workItemWithProjectAssociation in workItems)
        {
            _context.WorkItems.Add(workItemWithProjectAssociation.WorkItem);
            if (workItemWithProjectAssociation.Project is not null)
            {
                _logger.LogTrace($"Add association between work item {workItemWithProjectAssociation.WorkItem.Id} and project {workItemWithProjectAssociation.Project.Title}");
                _context.ProjectWorkItems.Add(new ProjectWorkItemDatabaseRecord(workItemWithProjectAssociation.Project.Id, workItemWithProjectAssociation.WorkItem.Id));
            }
        }

        _context.SaveChanges();
    }

    public void LinkProjectWithWorkItem(Guid projectId, int workItemId)
    {
        // TODO: override old value if exists
        if (_context.ProjectWorkItems.Any(pww => pww.ProjectId == projectId && pww.WorkItemId == workItemId))
            return;

        _context.ProjectWorkItems.Add(new ProjectWorkItemDatabaseRecord(projectId, workItemId));
        _context.SaveChanges();
    }
}