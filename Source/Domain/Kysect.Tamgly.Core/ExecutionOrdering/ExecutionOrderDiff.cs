namespace Kysect.Tamgly.Core;

public record struct ExecutionOrderDiff(WorkItem WorkItem, DateOnly Before, DateOnly After);