using Kysect.CommonLib.DateAndTime;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamgly.Common.IdentifierGenerators;
using Tamgly.Core.ExecutionOrdering;
using Tamgly.DataAccess;
using Tamgly.DataAccess.EntityFrameworkAdapter;
using Tamgly.DataAccess.Models;
using Tamgly.Integration.ExcelAdapter.Implementation;
using Tamgly.Integration.MicrosoftGraphAdapter.Authentication;
using Tamgly.Integration.MicrosoftGraphAdapter.Todo;
using Tamgly.Integration.MicrosoftGraphAdapter.Todo.ListToProjectMapping;
using Tamgly.Mapping;
using Tamgly.RepetitiveEvents.Models;
using Tamgly.RepetitiveEvents.Tools;
using Tamgly.ResultExport.Abstractions;
using Tamgly.WorkItemStorage.Abstractions;

namespace Tamgly.Playground.Scenarios;

public class TaskManagementWithAzureAndTodoWorkflow
{
    private readonly IReadOnlyCollection<IWorkItemStorage> _workItemStorages;
    private readonly TamglyDatabaseContext _databaseContext;
    private readonly IWorkItemExporter _workItemExporter;
    private readonly IExecutionOrderManager _executionOrderManager;

    private readonly MappingHolder _mappingHolder;
    private readonly ILogger _logger;

    public static async Task<TaskManagementWithAzureAndTodoWorkflow> CreateForAzureAndTodo(TamglyConfig config, ILogger logger)
    {
        IIdentifierGenerator identifierGenerator = InMemoryIdentifierGenerator.Instance;
        MicrosoftToDoWorkItemStorage microsoftToDoWorkItemStorage = await CreateTodoItemStorage(config, identifierGenerator);

        IReadOnlyCollection<IWorkItemStorage> workItemStorages = new IWorkItemStorage[] { microsoftToDoWorkItemStorage };
        TamglyDatabaseContext databaseContext = TamglyEntityFrameworkDbContextExtensions.CreateContext(new TamglyEntityFrameworkDbContext(), logger);
        IWorkItemExporter workItemExporter = new ExcelWorkItemExporter("Result.xlsx", true, logger);
        IExecutionOrderManager executionOrderManager = new ExecutionOrderManager(DateOnly.FromDateTime(DateTime.UtcNow), SelectedDayOfWeek.WorkDays, TimeSpan.FromHours(8), logger);


        return new TaskManagementWithAzureAndTodoWorkflow(
            workItemStorages,
            databaseContext,
            workItemExporter,
            executionOrderManager,
            MappingHolder.Instance,
            logger);
    }

    public static Task<TaskManagementWithAzureAndTodoWorkflow> CreateForOffice(TamglyConfig config, ILogger logger)
    {
        IReadOnlyCollection<IWorkItemStorage> workItemStorages = new IWorkItemStorage[] { };
        TamglyDatabaseContext databaseContext = TamglyEntityFrameworkDbContextExtensions.CreateContext(new TamglyEntityFrameworkDbContext(), logger);
        IWorkItemExporter workItemExporter = new ExcelWorkItemExporter("Result.xlsx", true, logger);
        IExecutionOrderManager executionOrderManager = new ExecutionOrderManager(DateOnly.FromDateTime(DateTime.UtcNow), SelectedDayOfWeek.WorkDays, TimeSpan.FromHours(8), logger);


        return Task.FromResult(new TaskManagementWithAzureAndTodoWorkflow(
            workItemStorages,
            databaseContext,
            workItemExporter,
            executionOrderManager,
            MappingHolder.Instance,
            logger));
    }

    private static async Task<MicrosoftToDoWorkItemStorage> CreateTodoItemStorage(TamglyConfig config, IIdentifierGenerator identifierGenerator)
    {
        var microsoftTodoTaskListFilter = new MicrosoftTodoTaskListToProjectMapper(config.MicrosoftTodo.ListToProjectMapping);
        var publicClientApplicationHolder = new PublicClientApplicationHolder(config.MicrosoftTodo.ClientId);
        var graphServiceClientFactory = new GraphServiceClientFactory(publicClientApplicationHolder);
        GraphServiceClient graphClient = await graphServiceClientFactory.CreateIntegratedWindowsAuth();
        return new MicrosoftToDoWorkItemStorage(graphClient, identifierGenerator, microsoftTodoTaskListFilter, MappingHolder.Instance);
    }

    public TaskManagementWithAzureAndTodoWorkflow(
        IReadOnlyCollection<IWorkItemStorage> workItemStorages,
        TamglyDatabaseContext databaseContext,
        IWorkItemExporter workItemExporter,
        IExecutionOrderManager executionOrderManager,
        MappingHolder mappingHolder,
        ILogger logger)
    {
        _workItemStorages = workItemStorages;
        _databaseContext = databaseContext;
        _workItemExporter = workItemExporter;
        _executionOrderManager = executionOrderManager;
        _mappingHolder = mappingHolder;
        _logger = logger;
    }

    public async Task Execute()
    {
        foreach (IWorkItemStorage workItemStorage in _workItemStorages)
        {
            await SaveToRepository(workItemStorage);
        }

        ExportToExcel();
        // TODO: fix
        //GenerateExecutionPlan();
    }

    private async Task SaveToRepository(IWorkItemStorage storage)
    {
        await storage.SyncWorkItems(_databaseContext);

        foreach (WorkItemDatabaseRecord workItem in _databaseContext.WorkItems.GetWorkItems())
        {
            _logger.LogInformation(_mappingHolder.WorkItems.Map(workItem).ToShortString());
        }
    }

    private void ExportToExcel()
    {
        IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> workItemDatabaseRecords = _databaseContext.WorkItems.GetWorkItemWithProjectAssociations();
        workItemDatabaseRecords = CreateWorkItemsFromRepetitive(workItemDatabaseRecords);
        var workItemWithProjectAssociations = _mappingHolder.WorkItemWithProjectAssociation.Map(workItemDatabaseRecords).ToList();
        _workItemExporter.ExportTaskList(workItemWithProjectAssociations);
    }

    private List<WorkItemWithProjectAssociationDatabaseRecord> CreateWorkItemsFromRepetitive(IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> workItemDatabaseRecords)
    {
        var result = new List<WorkItemWithProjectAssociationDatabaseRecord>();
        result.AddRange(workItemDatabaseRecords);

        foreach (WorkItemWithProjectAssociationDatabaseRecord workItem in workItemDatabaseRecords)
        {
            RepetitiveWorkItemConfigurationDatabaseRecord? repetitiveWorkItemConfiguration = _databaseContext.RepetitiveWorkItemConfigurations.Find(workItem.WorkItem.Id);
            if (repetitiveWorkItemConfiguration is null)
                continue;

            IRepetitiveEventPattern? repetitiveEventPattern = RepetitiveEventPatternSerializer.Instance.Deserialize(repetitiveWorkItemConfiguration.SerializedConfiguration, repetitiveWorkItemConfiguration.Type);
            ArgumentNullException.ThrowIfNull(repetitiveEventPattern);
            // TODO: config?
            var fromDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));

            var workItemsFromOccurrences = CreateForOccurrencesOnInterval(fromDate, endDate, workItem.WorkItem, repetitiveEventPattern)
                .Select(w => new WorkItemWithProjectAssociationDatabaseRecord(w, workItem.Project))
                .ToList();

            result.AddRange(workItemsFromOccurrences);
        }

        return result;
    }

    private IReadOnlyCollection<WorkItemDatabaseRecord> CreateForOccurrencesOnInterval(DateOnly from, DateOnly to, WorkItemDatabaseRecord source, IRepetitiveEventPattern repetitiveEventPattern)
    {
        OccurrenceGenerator occurrenceGenerator = OccurrenceGenerator.Instance;
        IReadOnlyCollection<DateOnly> occurrences = occurrenceGenerator.GetOccurrences(repetitiveEventPattern, from, to);
        return occurrences
            .Select(o => new WorkItemDatabaseRecord(
                source.Id,
                source.ExternalId,
                source.Title,
                source.Description,
                source.State,
                source.CreationTime,
                source.LastModifiedTime,
                source.CompletedTime,
                source.Estimate,
                source.DeadlineType,
                o,
                source.AssignedTo,
                source.Priority))
            .ToList();
    }

    private void GenerateExecutionPlan()
    {
        IReadOnlyCollection<WorkItemWithProjectAssociationDatabaseRecord> workItemDatabaseRecords = _databaseContext.WorkItems.GetWorkItemWithProjectAssociations();
        var workItemWithProjectAssociations = _mappingHolder.WorkItemWithProjectAssociation.Map(workItemDatabaseRecords).ToList();

        var itemDatabaseRecords = workItemWithProjectAssociations
            .ToLookup(g => g.Project?.Title, g => g.WorkItem)
            ["Backlog"]
            .ToList();

        ExecutionOrder executionOrder = _executionOrderManager.Order(itemDatabaseRecords);
        _workItemExporter.ExportExecutionOrder(executionOrder.Items.ToList());
    }
}