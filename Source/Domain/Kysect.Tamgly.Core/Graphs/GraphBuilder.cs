namespace Kysect.Tamgly.Core.Graphs;

public static class GraphBuilder
{
    public static GraphBuildResult<T> Build<T>(IReadOnlyCollection<Guid> nodes, List<GraphLink> links, GraphValueResolver<T> resolver)
    {
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

    private static GraphNode<T> BuildNode<T>(Guid id, ILookup<Guid, Guid> nodeLinks, GraphValueResolver<T> resolver)
    {
        IReadOnlyCollection<GraphNode<T>> child;
        if (!nodeLinks.Contains(id))
            child = Array.Empty<GraphNode<T>>();
        else
        {
            child = nodeLinks[id]
                .Select(childId => BuildNode(childId, nodeLinks, resolver))
                .ToList();
        }

        return new GraphNode<T>(id,  resolver.Resolve(id), child);
    }
}