using System.Collections.Generic;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo;

public class TamglyMicrosoftTodoConfig
{
    public string ClientId { get; init; }
    public Dictionary<string, string> ListToProjectMapping { get; init; }

    public TamglyMicrosoftTodoConfig(string clientId, Dictionary<string, string> listToProjectMapping)
    {
        ClientId = clientId;
        ListToProjectMapping = listToProjectMapping;
    }
}