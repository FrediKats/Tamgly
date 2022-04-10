using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Graphs;

public interface IGraphValueResolver<T>
{
    T Resolve(Guid id);
}

public class GraphValueResolver
{
    public static GraphValueResolver<T> Create<T>(IReadOnlyCollection<T> values, Func<T, Guid> selector)
    {
        return new GraphValueResolver<T>(values, selector);
    }
}

public class GraphValueResolver<T> : IGraphValueResolver<T>
{
    private readonly Dictionary<Guid, T> _map;

    public GraphValueResolver(IReadOnlyCollection<T> values, Func<T, Guid> selector)
    {
        _map = values.ToDictionary(selector, v => v);
    }

    public T Resolve(Guid id)
    {
        if (_map.TryGetValue(id, out var result))
            return result;

        throw new TamglyException($"Graph node with id {id} was not found");
    }
}