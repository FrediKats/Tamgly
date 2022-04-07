using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Aggregates;

public class WorkItemManager
{
    private readonly Project _defaultProject;
    private readonly WorkItemManagerConfig _config;
    private readonly ICollection<Project> _projects;

    public WorkItemManager() : this(new WorkItemManagerConfig(), new List<Project>())
    {
    }

    public WorkItemManager(WorkItemManagerConfig config, ICollection<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(projects);

        _config = config;
        _projects = projects;
        _defaultProject = new Project(Guid.Empty, "Default project", new List<WorkItem>());
        _projects.Add(_defaultProject);
    }

    public void AddWorkItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _defaultProject.AddItem(item);
    }

    public void RemoveWorkItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Project project = GetProject(item);
        project.RemoveItem(item);
    }

    public void AddProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

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

        Project oldProject = GetProject(item);
        oldProject.RemoveItem(item);
        project.AddItem(item);
    }

    public ICollection<Project> GetAllProjects()
    {
        return _projects;
    }

    public IReadOnlyCollection<WorkItem> GetWorkItems()
    {
        return _projects
            .SelectMany(p => p.Items)
            .ToList();
    }

    public IReadOnlyCollection<WorkItem> GetWorkItemsWithWrongEstimates()
    {
        return GetWorkItems()
            .Where(HasWrongEstimate)
            .ToList();

        bool HasWrongEstimate(WorkItem workItem)
        {
            double? matchPercent = workItem.TryGetEstimateMatchPercent();
            return matchPercent is not null
                   && matchPercent < _config.AcceptableEstimateDiff;
        }
    }

    private Project GetProject(WorkItem workItem)
    {
        return FindProject(workItem) ?? throw new TamglyException($"Work item was not matched with any project. Id: {workItem.Id}");
    }

    //TODO: WI32 for future optimizations
    private Project? FindProject(WorkItem workItem)
    {
        return _projects.SingleOrDefault(p => p.Items.Contains(workItem));
    }
}