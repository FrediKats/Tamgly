using System;
using System.Collections.Generic;
using Tamgly.DataAccess.Models;

namespace Tamgly.DataAccess.Repositories;

public interface IProjectRepository
{
    IReadOnlyCollection<ProjectDatabaseRecord> GetProjects();
    ProjectDatabaseRecord GetOrCreateProject(string title);
    void CreateWorkItems(IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> workItems);
    void LinkProjectWithWorkItem(Guid projectId, int workItemId);
}