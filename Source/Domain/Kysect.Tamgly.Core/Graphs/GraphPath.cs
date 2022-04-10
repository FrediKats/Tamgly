namespace Kysect.Tamgly.Core.Graphs;

public class GraphPath<T>
{
    public static GraphPath<T> Empty { get; } = new GraphPath<T>(Array.Empty<T>());

    public IReadOnlyCollection<T> Elements { get; }

    public GraphPath(IReadOnlyCollection<T> elements)
    {
        Elements = elements;
    }

    public GraphPath<T> AppendToStart(T value)
    {
        List<T> result = new List<T>();
        result.Add(value);
        result.AddRange(Elements);
        return new GraphPath<T>(result);
    }
}