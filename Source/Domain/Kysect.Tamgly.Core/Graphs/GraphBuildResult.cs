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
        foreach (GraphNode<T> graphNode in Roots)
        {
            GraphNode<T>? founded = graphNode.Find(id);
            if (founded is not null)
                return founded;
        }

        throw new TamglyException($"Work item with id {id} was not found");
    }
}