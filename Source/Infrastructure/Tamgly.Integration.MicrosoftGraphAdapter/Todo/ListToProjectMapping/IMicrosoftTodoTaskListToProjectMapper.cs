using System.Diagnostics.CodeAnalysis;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo.ListToProjectMapping;

public interface IMicrosoftTodoTaskListToProjectMapper
{
    bool IsAcceptable(string tile);
    bool TryMap(string title, [MaybeNullWhen(false)] out string projectName);
}