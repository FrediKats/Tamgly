namespace Kysect.Tamgly.Core;

public interface IExecutionOrderManager
{
    void Order(List<WorkItem> workItems, SelectedDayOfWeek selectedDayOfWeek, TimeSpan limitPerDay);
}