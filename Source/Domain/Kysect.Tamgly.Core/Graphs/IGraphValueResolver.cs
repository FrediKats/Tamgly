namespace Kysect.Tamgly.Core.Graphs;

public interface IGraphValueResolver<T>
{
    T Resolve(Guid id);
}