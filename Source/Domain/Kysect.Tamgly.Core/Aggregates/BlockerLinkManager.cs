using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Graphs;

namespace Kysect.Tamgly.Core.Aggregates;

public class BlockerLinkManager
{
    private readonly List<GraphLink> _links;
    private readonly WorkItemManager _workItemManager;

    /// <summary>
    /// Graph where children is WI's that block their parent.
    /// </summary>
    private GraphBuildResult<IWorkItem> _graphWhereChildrenBlockParent;

    /// <summary>
    /// Graph where children is WI's that blocked by their parent
    /// </summary>
    private GraphBuildResult<IWorkItem> _graphWhereParentBlockChildren;

    public BlockerLinkManager(WorkItemManager workItemManager)
    {
        ArgumentNullException.ThrowIfNull(workItemManager);

        _workItemManager = workItemManager;
        _links = new List<GraphLink>();
        _graphWhereChildrenBlockParent = RefreshGraph(false);
        _graphWhereParentBlockChildren = RefreshGraph(true);
    }

    public void AddLink(Guid from, Guid to)
    {
        _links.Add(new GraphLink(from, to));
        _graphWhereChildrenBlockParent = RefreshGraph(false);
        _graphWhereParentBlockChildren = RefreshGraph(true);
    }

    public bool IsBlocked(IWorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        GraphNode<IWorkItem> graphNode = _graphWhereParentBlockChildren.GetValue(workItem.Id);
        return graphNode.DirectChildren.Any();
    }

    private GraphBuildResult<IWorkItem> RefreshGraph(bool reverseLinks)
    {
        List<GraphLink> selectedLinks = reverseLinks
            ? _links.Select(l => l.Reversed()).ToList()
            : _links;

        IReadOnlyCollection<IWorkItem> workItems = _workItemManager.GetSelfWorkItems();
        var graphValueResolver = new GraphValueResolver<IWorkItem>(workItems, w => w.Id);
        return GraphBuilder.Build(workItems.Select(w => w.Id).ToList(), selectedLinks, graphValueResolver);
    }
}