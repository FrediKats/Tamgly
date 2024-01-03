using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.ExecutionOrdering;
using Tamgly.Core.Projects;
using Tamgly.Integration.ExcelAdapter.Contracts;
using Tamgly.Integration.ExcelAdapter.Models;
using Tamgly.ResultExport.Abstractions;

namespace Tamgly.Integration.ExcelAdapter.Implementation;

public class ExcelWorkItemExporter : IWorkItemExporter
{
    private readonly string _worksheetName;
    private readonly bool _removePreviousFile;
    private readonly ILogger _logger;

    public ExcelWorkItemExporter(string worksheetName, bool removePreviousFile, ILogger logger)
    {
        _worksheetName = worksheetName;
        _removePreviousFile = removePreviousFile;
        _logger = logger;
    }

    public void ExportTaskList(IReadOnlyList<WorkItemWithProjectAssociation> workItems)
    {
        new ExcelEntityExporter<WorkItemWithProjectAssociation>(
            _worksheetName,
            _removePreviousFile,
            new WorkItemExcelElementDecompose(_logger),
            new WorkItemExcelWorkbookSeparatorByState(),
            new WorkItemExcelElementSorter())
            .Export(workItems);
    }

    public void ExportExecutionOrder(IReadOnlyList<ExecutionOrderItem> executionOrderItems)
    {
        var list = executionOrderItems
            .SelectMany(WorkItemWithExecutionDate.From)
            .ToList();

        new ExcelEntityExporter<WorkItemWithExecutionDate>(
                _worksheetName,
                _removePreviousFile,
                new ExecutionOrderItemExcelElementDecompose(),
                ExcelWorkbookSeparatorExtensions.Default<WorkItemWithExecutionDate>(),
                ExcelElementSorterExtensions.Empty<WorkItemWithExecutionDate>())
            .Export(list);
    }
}