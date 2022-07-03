namespace Kysect.Tamgly.Graphs;

public static class GraphValueResolverCreator
{
    public static GraphValueResolver<T> Create<T>(IReadOnlyCollection<T> values, Func<T, Guid> selector)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(selector);

        return new GraphValueResolver<T>(values, selector);
    }
}