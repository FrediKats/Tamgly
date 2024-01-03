using System;
using System.ComponentModel.DataAnnotations;

namespace Tamgly.DataAccess.Models;

public class ProjectWorkItemDatabaseRecord
{
    public Guid ProjectId { get; set; }
    [Key]
    public int WorkItemId { get; set; }

    public ProjectWorkItemDatabaseRecord(Guid projectId, int workItemId) : this()
    {
        ProjectId = projectId;
        WorkItemId = workItemId;
    }

    public ProjectWorkItemDatabaseRecord()
    {
    }
}