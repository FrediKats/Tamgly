using System.Collections.Generic;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering;

public interface IExecutionOrderManager
{
    ExecutionOrder Order(IReadOnlyCollection<WorkItem> workItems);
}