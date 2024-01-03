namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo;

public struct MicrosoftTodoExternalId
{
    public string Value { get; }

    public MicrosoftTodoExternalId(string value)
    {
        Value = value;
    }

    public static MicrosoftTodoExternalId For(string todoInternalId)
    {
        return new MicrosoftTodoExternalId($"TODO-{todoInternalId}");
    }
}