namespace Tamgly.DataAccess.Models;

public class WorkItemWithProjectAssociationDatabaseRecord
{
    public WorkItemDatabaseRecord WorkItem { get; set; }
    public ProjectDatabaseRecord? Project { get; set; }

    public WorkItemWithProjectAssociationDatabaseRecord(WorkItemDatabaseRecord workItem, ProjectDatabaseRecord? project)
    {
        WorkItem = workItem;
        Project = project;
    }
}