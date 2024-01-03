using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Aggregates;

public class PrioritizedWorkItemManager : IWorkItemManager
{
    private readonly IWorkItemManager _workItemManager;
    private readonly BlockerLinkManager _blockerLinkManager;

    public PrioritizedWorkItemManager(IWorkItemManager workItemManager, BlockerLinkManager blockerLinkManager)
    {
        _workItemManager = workItemManager;
        _blockerLinkManager = blockerLinkManager;
    }

    public IReadOnlyCollection<WorkItem> GetSelfWorkItems()
    {
        return _workItemManager
            .GetSelfWorkItems()
            .Where(w => w.AssignedTo.IsMe())
            .Select(wi => _blockerLinkManager.GetWithTotalPriority(wi))
            .OrderBy(wi => wi.Priority)
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetAllWorkItems()
    {
        return _workItemManager
            .GetAllWorkItems()
            .Select(wi => _blockerLinkManager.GetWithTotalPriority(wi))
            .OrderBy(wi => wi.Priority)
            .ToList();
    }
}