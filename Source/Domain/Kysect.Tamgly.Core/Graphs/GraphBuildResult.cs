using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Graphs;

public class GraphBuildResult<T>
{
    public GraphBuildResult(IReadOnlyCollection<GraphNode<T>> roots)
    {
        ArgumentNullException.ThrowIfNull(roots);

        Roots = roots;
    }

    public IReadOnlyCollection<GraphNode<T>> Roots { get; }

    public GraphNode<T> GetValue(Guid id)
    {
        GraphNode<T>? found = Roots
            .Select(root => root.Find(id))
            .FirstOrDefault(value => value is not null);

        return found ?? throw new TamglyException($"Work item with id {id} was not found");
    }
}