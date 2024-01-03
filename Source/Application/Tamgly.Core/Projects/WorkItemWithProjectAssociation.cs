using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Projects;

public record WorkItemWithProjectAssociation(WorkItem WorkItem, Project? Project);