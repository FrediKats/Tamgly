namespace Kysect.Tamgly.Core.Graphs;

public record struct GraphLink(Guid From, Guid To)
{
    public GraphLink Reverse() => new GraphLink(From: To, To: From);
}