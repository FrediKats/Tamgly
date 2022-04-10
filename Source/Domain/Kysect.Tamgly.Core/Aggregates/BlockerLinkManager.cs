using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Graphs;

namespace Kysect.Tamgly.Core.Aggregates;

public class BlockerLinkManager
{
    private readonly List<GraphLink> _links;
    private readonly WorkItemManager _workItemManager;
    private GraphBuildResult<IWorkItem> _blockingGraph;
    private GraphBuildResult<IWorkItem> _blockedGraph;

    public BlockerLinkManager(WorkItemManager workItemManager)
    {
        ArgumentNullException.ThrowIfNull(workItemManager);

        _workItemManager = workItemManager;
        _links = new List<GraphLink>();
        _blockingGraph = RefreshGraph();
        _blockedGraph = RefreshReverseGraph();
    }

    public void AddLink(Guid from, Guid to)
    {
        _links.Add(new GraphLink(from, to));
        _blockingGraph = RefreshGraph();
        _blockedGraph = RefreshReverseGraph();
    }

    public bool IsBlocked(IWorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        GraphNode<IWorkItem> graphNode = _blockedGraph.GetValue(workItem.Id);
        return graphNode.DirectChild.Any();
    }

    private GraphBuildResult<IWorkItem> RefreshGraph()
    {
        IReadOnlyCollection<IWorkItem> workItems = _workItemManager.GetWorkItems();
        var graphValueResolver = new GraphValueResolver<IWorkItem>(workItems, w => w.Id);
        return GraphBuilder.Build(workItems.Select(w => w.Id).ToList(), _links, graphValueResolver);
    }

    private GraphBuildResult<IWorkItem> RefreshReverseGraph()
    {
        IReadOnlyCollection<IWorkItem> workItems = _workItemManager.GetWorkItems();
        var graphValueResolver = new GraphValueResolver<IWorkItem>(workItems, w => w.Id);
        return GraphBuilder.Build(
            workItems.Select(w => w.Id).ToList(),
            _links.Select(l => l.Reverse()).ToList(),
            graphValueResolver);
    }
}