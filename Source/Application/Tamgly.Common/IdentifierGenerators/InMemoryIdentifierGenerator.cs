namespace Tamgly.Common.IdentifierGenerators;

public class InMemoryIdentifierGenerator : IIdentifierGenerator
{
    public static InMemoryIdentifierGenerator Instance { get; } = new InMemoryIdentifierGenerator();

    private int _next;

    public InMemoryIdentifierGenerator(int first)
    {
        _next = first;
    }

    public InMemoryIdentifierGenerator() : this(1)
    {
    }

    public int GetNext()
    {
        int value = _next;
        _next++;
        return value;
    }
}