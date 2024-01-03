using System.Collections.Generic;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Aggregates;

public interface IWorkItemManager
{
    IReadOnlyCollection<WorkItem> GetSelfWorkItems();
    IReadOnlyCollection<WorkItem> GetAllWorkItems();
}