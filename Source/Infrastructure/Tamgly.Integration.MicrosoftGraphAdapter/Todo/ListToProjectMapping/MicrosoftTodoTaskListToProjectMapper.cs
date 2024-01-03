using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo.ListToProjectMapping;

public class MicrosoftTodoTaskListToProjectMapper : IMicrosoftTodoTaskListToProjectMapper
{
    private readonly Dictionary<string, string> _listToProjectMapping;

    public MicrosoftTodoTaskListToProjectMapper(Dictionary<string, string> listToProjectMapping)
    {
        _listToProjectMapping = listToProjectMapping;
    }

    public bool IsAcceptable(string tile)
    {
        return _listToProjectMapping.ContainsKey(tile);
    }

    public bool TryMap(string title, [MaybeNullWhen(false)] out string projectName)
    {
        return _listToProjectMapping.TryGetValue(title, out projectName);
    }
}