namespace Kysect.Tamgly.Graphs;

public interface IGraphValueResolver<T>
{
    T Resolve(Guid id);
}