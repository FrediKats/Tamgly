using Kysect.CommonLib.BaseTypes.Extensions;
using Tamgly.Core.Projects;
using Tamgly.DataAccess.Models;

namespace Tamgly.Mapping.Implementations;

public class WorkItemWithProjectAssociationMapper : IMapper<WorkItemWithProjectAssociation, WorkItemWithProjectAssociationDatabaseRecord>
{
    private readonly WorkItemMapper _workItemMapper;
    private readonly ProjectMapper _projectMapper;

    public WorkItemWithProjectAssociationMapper(WorkItemMapper workItemMapper, ProjectMapper projectMapper)
    {
        _workItemMapper = workItemMapper;
        _projectMapper = projectMapper;
    }

    public WorkItemWithProjectAssociationDatabaseRecord Map(WorkItemWithProjectAssociation value)
    {
        return new WorkItemWithProjectAssociationDatabaseRecord(
            _workItemMapper.Map(value.WorkItem),
            value.Project?.To(_projectMapper.Map));
    }

    public WorkItemWithProjectAssociation Map(WorkItemWithProjectAssociationDatabaseRecord value)
    {
        return new WorkItemWithProjectAssociation(
            _workItemMapper.Map(value.WorkItem),
            value.Project?.To(_projectMapper.Map));
    }
}