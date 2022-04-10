namespace Kysect.Tamgly.Core.Graphs;

public class GraphNode<T>
{
    public Guid Id { get; set; }
    public T Value { get; }
    public IReadOnlyCollection<GraphNode<T>> DirectChild { get; set; }

    public GraphNode(Guid id, T value, IReadOnlyCollection<GraphNode<T>> directChild)
    {
        ArgumentNullException.ThrowIfNull(directChild);

        Id = id;
        Value = value;
        DirectChild = directChild;
    }

    public IReadOnlyCollection<GraphPath<T>> EnumeratePathToLeaves()
    {
        if (!DirectChild.Any())
        {
            return new[] { GraphPath<T>.Empty.AppendToStart(Value) };
        }

        List<GraphPath<T>> result = new List<GraphPath<T>>();
        foreach (GraphNode<T> child in DirectChild)
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

        foreach (GraphNode<T> node in DirectChild)
        {
            var founded = node.Find(id);
            if (founded is not null)
                return founded;
        }

        return null;
    }
}