namespace Kysect.Tamgly.Graphs;

public static class GraphBuilder
{
    public static GraphBuildResult<T> Build<T>(
        IReadOnlyCollection<Guid> nodes,
        IReadOnlyCollection<GraphLink> links,
        GraphValueResolver<T> resolver)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNull(links);
        ArgumentNullException.ThrowIfNull(resolver);

        HashSet<Guid> targetNodes = links
            .Select(l => l.To)
            .ToHashSet();

        List<Guid> rootNodes = nodes
            .Where(l => !targetNodes.Contains(l))
            .ToList();

        ILookup<Guid, Guid> nodeLinks = links.ToLookup(l => l.From, l => l.To);

        List<GraphNode<T>> result = new List<GraphNode<T>>();

        foreach (Guid rootNode in rootNodes)
            result.Add(BuildNode(rootNode, nodeLinks, resolver));

        return new GraphBuildResult<T>(result);
    }

    private static GraphNode<T> BuildNode<T>(Guid id, ILookup<Guid, Guid> nodeLinks, IGraphValueResolver<T> resolver)
    {
        IReadOnlyCollection<GraphNode<T>> children = nodeLinks.Contains(id)
            ? BuildChildren(nodeLinks[id], nodeLinks, resolver)
            : Array.Empty<GraphNode<T>>();
        return new GraphNode<T>(id, resolver.Resolve(id), children);
    }
    private static IReadOnlyCollection<GraphNode<T>> BuildChildren<T>(
        IEnumerable<Guid> identifiers,
        ILookup<Guid, Guid> nodeLinks,
        IGraphValueResolver<T> resolver)
    {
        return identifiers
            .Select(childId => BuildNode(childId, nodeLinks, resolver))
            .ToList();
    }
}