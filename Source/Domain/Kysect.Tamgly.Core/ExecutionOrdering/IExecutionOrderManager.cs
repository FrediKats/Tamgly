using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public interface IExecutionOrderManager
{
    IReadOnlyCollection<DailyAssignments> Order(List<WorkItem> workItems, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay);
}