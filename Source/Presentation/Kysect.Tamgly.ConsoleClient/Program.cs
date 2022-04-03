// See https://aka.ms/new-console-template for more information

using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.ValueObjects;

var workItemManager = new WorkItemManager();
var workItem = WorkItem.Create("Support projects");
workItemManager.AddWorkItem(workItem);

var project = Project.Create("Tamgly");
workItemManager.AddProject(project);
workItemManager.ChangeProject(workItem, project);
Console.WriteLine($"Project WI count: {project.Items.Count}");

var backlogManager = new BacklogManager(workItemManager);
DateTime workItemDeadline = DateTime.Today.AddDays(10);
workItem.Deadline = WorkItemDeadline.Create(WorkItemDeadline.Type.Day, workItemDeadline);
WorkItemBacklog workItemBacklog = backlogManager.GetDailyBacklog(workItemDeadline);
Console.WriteLine($"Daily backlog WI count: {workItemBacklog.Items.Count}");

WorkItemBacklog weeklyBacklog = backlogManager.GetWeeklyBacklog(workItemDeadline);
Console.WriteLine($"Weekly backlog WI count: {weeklyBacklog.Items.Count}");
return;