using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo.ListToProjectMapping;

public static class MicrosoftTodoTaskListToProjectMapperExtensions
{
    public static string Map(this IMicrosoftTodoTaskListToProjectMapper mapper, string listTitle)
    {
        if (mapper.TryMap(listTitle, out string? result))
            return result;

        throw new Exception($"List {listTitle} does not have mapping.");
    }

    public static ILookup<string, TodoTask> GroupByProject(this IMicrosoftTodoTaskListToProjectMapper mapper, Dictionary<string, List<TodoTask>> tasks)
    {
        ILookup<string, TodoTask> taskGroupedByProject = tasks
            .Where(p => mapper.IsAcceptable(p.Key))
            .SelectMany(p => p.Value.Select(v => (Project: mapper.Map(p.Key), WorkItem: v)))
            .ToLookup(p => p.Project, p => p.WorkItem);

        return taskGroupedByProject;
    }
}