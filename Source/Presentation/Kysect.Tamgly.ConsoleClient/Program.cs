// See https://aka.ms/new-console-template for more information

using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities;

var workItemManager = new WorkItemManager();
var workItem = WorkItem.Create("Support projects");
workItemManager.AddWorkItem(workItem);

var project = Project.Create("Tamgly");
workItemManager.AddProject(project);
workItemManager.ChangeProject(workItem, project);
Console.WriteLine($"Project WI count: {project.Items.Count}");

var backlogManager = new BacklogManager(workItemManager);
DateTime workItemDeadline = DateTime.Today.AddDays(10);
workItem.Deadline = workItemDeadline;
DailyBacklog dailyBacklog = backlogManager.GetDailyBacklog(workItemDeadline);
Console.WriteLine($"Daily backlog WI count: {dailyBacklog.Items.Count}");