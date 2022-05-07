using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.ValueObjects;
using Kysect.Tamgly.Graphs;
using Serilog;

namespace Kysect.Tamgly.Core.Aggregates;

public class BlockerLinkManager
{
    private readonly List<GraphLink> _links;
    private readonly WorkItemManager _workItemManager;

    /// <summary>
    /// Graph where children is WI's that block their parent.
    /// </summary>
    private GraphBuildResult<WorkItem> _graphWhereChildrenBlockParent;

    /// <summary>
    /// Graph where children is WI's that blocked by their parent
    /// </summary>
    private GraphBuildResult<WorkItem> _graphWhereParentBlockChildren;

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
        Log.Verbose($"Add new dependency link: {from} {to}");

        _links.Add(new GraphLink(from, to));

        _graphWhereChildrenBlockParent = RefreshGraph(false);
        _graphWhereParentBlockChildren = RefreshGraph(true);
    }

    public bool IsBlocked(WorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        GraphNode<WorkItem> graphNode = _graphWhereParentBlockChildren.GetValue(workItem.Id);
        return graphNode.EnumerateChildren().Any();
    }

    public WorkItemPriority? CalculateTotalPriority(WorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        GraphNode<WorkItem> graphNode = _graphWhereChildrenBlockParent.GetValue(workItem.Id);
        WorkItemPriority? childrenMaxPriority = graphNode
            .EnumerateChildren()
            .Where(c => c.Value.Priority is not null)
            .Max(c => c.Value.Priority);

        if (childrenMaxPriority is null)
            return workItem.Priority;

        if (workItem.Priority is not null)
            return childrenMaxPriority;

        return workItem.Priority > childrenMaxPriority ? workItem.Priority : childrenMaxPriority;
    }

    private GraphBuildResult<WorkItem> RefreshGraph(bool reverseLinks)
    {
        List<GraphLink> selectedLinks = reverseLinks
            ? _links.Select(l => l.Reversed()).ToList()
            : _links;

        IReadOnlyCollection<WorkItem> workItems = _workItemManager.GetSelfWorkItems();
        var graphValueResolver = new GraphValueResolver<WorkItem>(workItems, w => w.Id);
        return GraphBuilder.Build(workItems.Select(w => w.Id).ToList(), selectedLinks, graphValueResolver);
    }
}