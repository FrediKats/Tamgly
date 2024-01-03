using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo;

public class MicrosoftTodoWalker
{
    private readonly GraphServiceClient _client;

    public MicrosoftTodoWalker(GraphServiceClient client)
    {
        _client = client;
    }

    public async Task<Dictionary<string, List<TodoTask>>> GetAllTasks()
    {
        var todoTasksDict = new Dictionary<string, List<TodoTask>>();

        List<TodoTaskList> currentLists = await GetAllTodoLists();

        foreach (TodoTaskList todoList in currentLists)
        {
            string? todoListDisplayName = todoList.DisplayName;
            if (todoListDisplayName is null)
                throw new ArgumentException($"Item {todoList.Id} has null name");

            todoTasksDict[todoListDisplayName] = await GetAllTodoListTasks(todoList);
        }

        return todoTasksDict;
    }

    private async Task<List<TodoTaskList>> GetAllTodoLists()
    {
        var currentLists = new List<TodoTaskList>();

        Microsoft.Graph.Models.Todo? todoListsPage = await _client.Me.Todo.GetAsync();
        todoListsPage.ThrowIfNull(nameof(todoListsPage));
        todoListsPage.Lists.ThrowIfNull(nameof(todoListsPage.Lists));
        return todoListsPage.Lists;

        //currentLists.AddRange(todoListsPage.CurrentPage.ToList());
        //while (todoListsPage.NextPageRequest != null)
        //{
        //    todoListsPage = await todoListsPage.NextPageRequest.GetAsync();
        //    currentLists.AddRange(todoListsPage.CurrentPage.ToList());
        //}

        //return currentLists;        //currentLists.AddRange(todoListsPage.CurrentPage.ToList());
        //while (todoListsPage.NextPageRequest != null)
        //{
        //    todoListsPage = await todoListsPage.NextPageRequest.GetAsync();
        //    currentLists.AddRange(todoListsPage.CurrentPage.ToList());
        //}

        //return currentLists;
    }

    private async Task<List<TodoTask>> GetAllTodoListTasks(TodoTaskList todoList)
    {
        var todoTasks = new List<TodoTask>();
        TodoTaskCollectionResponse? todoTasksPage = await _client.Me.Todo.Lists[todoList.Id].Tasks.GetAsync();
        todoTasksPage.ThrowIfNull(nameof(todoTasksPage));
        todoTasksPage.Value.ThrowIfNull(nameof(todoTasksPage.Value));
        return todoTasksPage.Value;
        //todoTasks.AddRange(todoTasksPage.CurrentPage.ToList());
        //while (todoTasksPage.NextPageRequest != null)
        //{
        //    todoTasksPage = await todoTasksPage.NextPageRequest.GetAsync();
        //    todoTasks.AddRange(todoTasksPage.CurrentPage.ToList());
        //}

        //return todoTasks;
    }
}