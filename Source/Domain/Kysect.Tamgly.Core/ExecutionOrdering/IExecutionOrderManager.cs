using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public interface IExecutionOrderManager
{
    ExecutionOrder Order(IReadOnlyCollection<WorkItem> workItems, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay);
}