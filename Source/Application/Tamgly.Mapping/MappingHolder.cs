using Tamgly.Mapping.Implementations;

namespace Tamgly.Mapping;

public class MappingHolder
{
    public MappingHolder()
    {
        WorkItemTrackInterval = new WorkItemTrackIntervalMapper();
        WorkItems = new WorkItemMapper(WorkItemTrackInterval);
        Project = new ProjectMapper();
        WorkItemWithProjectAssociation = new WorkItemWithProjectAssociationMapper(WorkItems, Project);
    }

    public static MappingHolder Instance { get; } = new MappingHolder();

    public WorkItemTrackIntervalMapper WorkItemTrackInterval { get; }
    public WorkItemMapper WorkItems { get; }
    public ProjectMapper Project { get; }
    public WorkItemWithProjectAssociationMapper WorkItemWithProjectAssociation { get; }
}