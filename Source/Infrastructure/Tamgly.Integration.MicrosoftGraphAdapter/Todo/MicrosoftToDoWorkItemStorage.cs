using Kysect.CommonLib.BaseTypes.Extensions;
using Tamgly.Integration.MicrosoftGraphAdapter.Todo.ListToProjectMapping;
using Tamgly.WorkItemStorage.Abstractions;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.WorkItems;
using Tamgly.DataAccess;
using Tamgly.DataAccess.Models;
using Tamgly.DataAccess.Repositories;
using Tamgly.Mapping;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Todo;

public class MicrosoftToDoWorkItemStorage : IWorkItemStorage
{
    private readonly IMicrosoftTodoTaskListToProjectMapper _listToProjectMapper;
    private readonly MicrosoftTodoWalker _microsoftTodoWalker;
    private readonly TodoTaskToWorkItemMapper _todoTaskToWorkItemMapper;
    private readonly MappingHolder _workItemMapper;

    public MicrosoftToDoWorkItemStorage(GraphServiceClient client, IIdentifierGenerator identifierGenerator, IMicrosoftTodoTaskListToProjectMapper listToProjectMapper, MappingHolder workItemMapper)
    {
        _listToProjectMapper = listToProjectMapper;
        _workItemMapper = workItemMapper;
        _microsoftTodoWalker = new MicrosoftTodoWalker(client);
        _todoTaskToWorkItemMapper = new TodoTaskToWorkItemMapper(identifierGenerator);
    }

    public async Task SyncWorkItems(TamglyDatabaseContext databaseContext)
    {
        Dictionary<string, List<TodoTask>> itemsFromStorage = await _microsoftTodoWalker.GetAllTasks();

        foreach ((string internalProjectName, List<TodoTask> tasks) in itemsFromStorage)
        {
            foreach (TodoTask todoTask in tasks)
            {
                string? taskId = todoTask.Id;
                taskId.ThrowIfNull();

                var externalId = MicrosoftTodoExternalId.For(taskId);
                WorkItemDatabaseRecord? workItemDatabaseRecord = databaseContext.WorkItems.FindByExternalId(externalId.Value);
                if (workItemDatabaseRecord is not null)
                {
                    // TODO: update work item
                }
                else
                {
                    WorkItem mapperWorkItem = _todoTaskToWorkItemMapper.Convert(todoTask);
                    workItemDatabaseRecord = _workItemMapper.WorkItems.Map(mapperWorkItem);
                    databaseContext.WorkItems.AddWorkItem(workItemDatabaseRecord);
                }

                if (_listToProjectMapper.TryMap(internalProjectName, out string? realProjectName))
                {
                    ProjectDatabaseRecord projectDatabaseRecord = databaseContext.Projects.GetOrCreateProject(realProjectName);
                    databaseContext.Projects.LinkProjectWithWorkItem(projectDatabaseRecord.Id, workItemDatabaseRecord.Id);
                }
            }
        }
    }
}