using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Common.Exceptions;
using Tamgly.Core.Projects;
using Tamgly.Core.TimeTracking;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Aggregates;

public class WorkItemManager : IWorkItemManager
{
    private readonly Project _defaultProject;
    private readonly WorkItemManagerConfig _config;
    private readonly ICollection<Project> _projects;
    private readonly IWorkItemTrackingIntervalProvider _workItemTrackingIntervalProvider;
    private readonly ILogger _logger;

    public WorkItemManager(ILogger logger, IWorkItemTrackingIntervalProvider workItemTrackingIntervalProvider)
        : this(new WorkItemManagerConfig(), new List<Project>(), logger, workItemTrackingIntervalProvider)
    {
    }

    public WorkItemManager(WorkItemManagerConfig config, ICollection<Project> projects, ILogger logger, IWorkItemTrackingIntervalProvider workItemTrackingIntervalProvider)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(projects);

        _config = config;
        _projects = projects;
        _logger = logger;
        _workItemTrackingIntervalProvider = workItemTrackingIntervalProvider;
        _defaultProject = new Project(Guid.Empty, "Default project", new List<WorkItem>(), WorkingHours.Empty);
        _projects.Add(_defaultProject);
    }

    public void AddWorkItem(WorkItem item, Project? project = null)
    {
        ArgumentNullException.ThrowIfNull(item);

        _logger.LogInformation($"Add WI {item.Id} to WIManager");

        _defaultProject.AddItem(item);

        if (project is not null)
            ChangeProject(item, project);
    }

    public void RemoveWorkItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Project project = GetProject(item);
        project.RemoveItem(item);
    }

    public void UpdateWorkItem(WorkItem item)
    {
        _logger.LogInformation($"Update WI in WIManager: {item.ToShortString()}");

        Project? project = FindProject(item);
        if (project is null)
            return;

        project.RemoveItem(item);
        project.AddItem(item);
    }

    public void AddProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        _logger.LogInformation($"Add new project {project.ToShortString()} to WIManager");

        _projects.Add(project);
    }

    public void RemoveProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (project.Id == _defaultProject.Id)
            throw new TamglyException("Cannot remove default project.");

        if (!_projects.Remove(project))
            throw new TamglyException($"Project was not found. Id: {project.Id}");

        foreach (WorkItem projectItem in project.Items)
            _defaultProject.AddItem(projectItem);
    }

    public void ChangeProject(WorkItem item, Project project)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(project);

        _logger.LogInformation($"Change project for {item.ToShortString()} to {project.ToShortString()}");

        Project oldProject = GetProject(item);
        oldProject.RemoveItem(item);
        project.AddItem(item);
    }

    public ICollection<Project> GetAllProjects()
    {
        return _projects;
    }

    public IReadOnlyCollection<WorkItem> GetSelfWorkItems()
    {
        return _projects
            .SelectMany(p => p.Items)
            .Where(w => w.AssignedTo.IsMe())
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetAllWorkItems()
    {
        return _projects
            .SelectMany(p => p.Items)
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetMisestimatedWorkItems()
    {
        return GetSelfWorkItems()
            .Where(IsMisestimated)
            .ToList();

        bool IsMisestimated(WorkItem workItem)
        {
            IReadOnlyCollection<WorkItemTrackInterval> workItemTrackIntervals = _workItemTrackingIntervalProvider.GetIntervals(workItem);
            double? matchPercent = workItem.TryGetEstimateMatchPercent(workItemTrackIntervals);
            return matchPercent is not null
                   && matchPercent < _config.AcceptableEstimateDiff;
        }
    }

    private Project GetProject(WorkItem workItem)
    {
        return FindProject(workItem) ?? throw new TamglyException($"Work item was not matched with any project. Id: {workItem.Id}");
    }

    // TODO: WI32 for future optimizations
    private Project? FindProject(WorkItem workItem)
    {
        return _projects.SingleOrDefault(p => p.Items.Contains(workItem));
    }
}