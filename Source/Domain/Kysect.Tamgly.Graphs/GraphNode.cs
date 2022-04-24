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

    public IReadOnlyCollection<GraphPath<T>> EnumeratePathToLeaves()
    {
        if (!DirectChildren.Any())
        {
            return new[] { GraphPath<T>.Empty.AppendToStart(Value) };
        }

        List<GraphPath<T>> result = new List<GraphPath<T>>();
        foreach (GraphNode<T> child in DirectChildren)
        {
            foreach (GraphPath<T> pathToLeaf in child.EnumeratePathToLeaves())
            {
                result.Add(pathToLeaf.AppendToStart(child.Value));
            }
        }

        return result;
    }

    public GraphNode<T>? Find(Guid id)
    {
        if (Id == id)
            return this;

        return DirectChildren
            .Select(node => node.Find(id))
            .FirstOrDefault(founded => founded is not null);
    }
}