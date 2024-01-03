using Tamgly.DataAccess.Repositories;

namespace Tamgly.DataAccess;

public class TamglyDatabaseContext
{
    public IWorkItemRepository WorkItems { get; }
    public IProjectRepository Projects { get; }
    public IRepetitiveWorkItemConfigurationRepository RepetitiveWorkItemConfigurations { get; }
    public IWorkItemTrackingIntervalRepository WorkItemsTrackingIntervals { get; }

    public TamglyDatabaseContext(
        IWorkItemRepository workItems,
        IProjectRepository projects,
        IRepetitiveWorkItemConfigurationRepository repetitiveWorkItemConfigurations,
        IWorkItemTrackingIntervalRepository workItemsTrackingIntervals)
    {
        WorkItems = workItems;
        Projects = projects;
        RepetitiveWorkItemConfigurations = repetitiveWorkItemConfigurations;
        WorkItemsTrackingIntervals = workItemsTrackingIntervals;
    }
}