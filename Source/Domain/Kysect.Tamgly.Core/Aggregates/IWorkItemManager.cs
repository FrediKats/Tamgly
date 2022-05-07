namespace Kysect.Tamgly.Core;

public interface IWorkItemManager
{
    IReadOnlyCollection<WorkItem> GetSelfWorkItems();
    IReadOnlyCollection<WorkItem> GetAllWorkItems();
}