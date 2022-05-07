namespace Kysect.Tamgly.Graphs;

public class GraphNode<T>
{
    public Guid Id { get; }
    public T Value { get; }
    public IReadOnlyCollection<GraphNode<T>> DirectChildren { get; }

    public GraphNode(Guid id, T value, IReadOnlyCollection<GraphNode<T>> directChildren)
    {
        ArgumentNullException.ThrowIfNull(directChildren);

        Id = id;
        Value = value;
        DirectChildren = directChildren;
    }

    public IEnumerable<GraphNode<T>> EnumerateChildren()
    {
        if (!DirectChildren.Any())
            return Array.Empty<GraphNode<T>>();

        return DirectChildren
            .Concat(DirectChildren.SelectMany(c => c.EnumerateChildren()))
            .ToList();
    }

    public GraphNode<T>? Find(Guid id)
    {
        if (Id == id)
            return this;

        return DirectChildren
            .Select(node => node.Find(id))
            .FirstOrDefault(founded => founded is not null);
    }

    public override string ToString()
    {
        return $"Node {Value}, Direct children count: {DirectChildren.Count}";
    }
}