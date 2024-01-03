using System;
using Tamgly.DataAccess.Models;

namespace Tamgly.DataAccess.EntityFrameworkAdapter.Models;

public class WorkItemWithProjectId
{
    public WorkItemDatabaseRecord WorkItem { get; set; }
    public Guid ProjectId { get; set; }

    public WorkItemWithProjectId(WorkItemDatabaseRecord workItem, Guid projectId)
    {
        WorkItem = workItem;
        ProjectId = projectId;
    }
}