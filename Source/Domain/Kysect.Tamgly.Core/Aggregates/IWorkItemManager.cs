using Kysect.Tamgly.Core.Entities;

namespace Kysect.Tamgly.Core.Aggregates;

public interface IWorkItemManager
{
    IReadOnlyCollection<WorkItem> GetSelfWorkItems();
    IReadOnlyCollection<WorkItem> GetAllWorkItems();
}