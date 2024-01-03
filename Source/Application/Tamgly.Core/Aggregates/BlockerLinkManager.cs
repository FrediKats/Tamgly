using Kysect.CommonLib.Graphs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Aggregates;

public class BlockerLinkManager
{
    private readonly List<GraphLink<int>> _links;
    private readonly WorkItemManager _workItemManager;
    private readonly ILogger _logger;

    /// <summary>
    /// Graph where children is WI's that block their parent.
    /// </summary>
    private GraphBuildResult<int, WorkItem> GraphWhereChildrenBlockParent => RefreshGraph(false);

    /// <summary>
    /// Graph where children is WI's that blocked by their parent
    /// </summary>
    private GraphBuildResult<int, WorkItem> GraphWhereParentBlockChildren => RefreshGraph(true);

    public BlockerLinkManager(WorkItemManager workItemManager, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(workItemManager);

        _workItemManager = workItemManager;
        _logger = logger;

        _links = new List<GraphLink<int>>();
    }

    public void AddLink(int from, int to)
    {
        _logger.LogTrace($"Add new dependency link: {from} {to}");

        _links.Add(new GraphLink<int>(from, to));
    }

    public bool IsBlocked(WorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        GraphNode<int, WorkItem> graphNode = GraphWhereParentBlockChildren.GetValue(workItem.Id);
        return graphNode.EnumerateChildren().Any();
    }

    public WorkItem GetWithTotalPriority(WorkItem workItem)
    {
        workItem.Priority = CalculateTotalPriority(workItem);
        return workItem;
    }

    public WorkItemPriority? CalculateTotalPriority(WorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        GraphNode<int, WorkItem> graphNode = GraphWhereChildrenBlockParent.GetValue(workItem.Id);
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

    private GraphBuildResult<int, WorkItem> RefreshGraph(bool reverseLinks)
    {
        List<GraphLink<int>> selectedLinks = reverseLinks
            ? _links.Select(l => l.Reversed()).ToList()
            : _links;

        IReadOnlyCollection<WorkItem> workItems = _workItemManager.GetSelfWorkItems();
        var graphValueResolver = new GraphValueResolver<int, WorkItem>(workItems, w => w.Id);
        return GraphBuilder.Build(workItems.Select(w => w.Id).ToList(), selectedLinks, graphValueResolver);
    }
}