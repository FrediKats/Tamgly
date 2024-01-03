using System.Collections.Generic;
using Tamgly.Core.ExecutionOrdering;
using Tamgly.Core.Projects;

namespace Tamgly.ResultExport.Abstractions;

public interface IWorkItemExporter
{
    void ExportTaskList(IReadOnlyList<WorkItemWithProjectAssociation> workItems);
    void ExportExecutionOrder(IReadOnlyList<ExecutionOrderItem> executionOrderItems);
}