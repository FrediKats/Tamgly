namespace Kysect.Tamgly.Graphs;

public record struct GraphLink(Guid From, Guid To)
{
    public GraphLink Reversed() => new GraphLink(From: To, To: From);
}