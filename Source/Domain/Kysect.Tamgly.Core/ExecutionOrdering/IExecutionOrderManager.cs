namespace Kysect.Tamgly.Core;

public interface IExecutionOrderManager
{
    ExecutionOrder Order(IReadOnlyCollection<WorkItem> workItems);
}