using System;
using Tamgly.Core.Projects;
using Tamgly.Core.WorkItems;
using Tamgly.DataAccess.Models;

namespace Tamgly.Mapping.Implementations;

public class ProjectMapper : IMapper<Project, ProjectDatabaseRecord>
{
    public ProjectDatabaseRecord Map(Project value)
    {
        return new ProjectDatabaseRecord(
            value.Id,
            value.Title,
            value.WorkingHours.PerDay,
            value.WorkingHours.SelectedDays,
            value.WorkingHours.PerWeek,
            value.WorkingHours.PerMonth);
    }

    public Project Map(ProjectDatabaseRecord value)
    {
        return new Project(
            value.Id,
            value.Title,
            Array.Empty<WorkItem>(),
            new WorkingHours(
                value.PerDay,
                value.SelectedDays,
                value.PerWeek,
                value.PerMonth));
    }
}