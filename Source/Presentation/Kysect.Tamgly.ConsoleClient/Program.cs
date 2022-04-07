using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

var workItemManager = new WorkItemManager();
var workItemBuilder = new WorkItemBuilder("Support projects");
WorkItem workItem = workItemBuilder.Build();
workItemManager.AddWorkItem(workItem);

var project = Project.Create("Tamgly");
workItemManager.AddProject(project);
workItemManager.ChangeProject(workItem, project);
Console.WriteLine($"Project WI count: {project.Items.Count}");

var backlogManager = new BacklogManager(workItemManager);
DateOnly workItemDeadline = DateOnly.FromDateTime(DateTime.Today).AddDays(10);
workItem.Deadline = WorkItemDeadline.Create(WorkItemDeadlineType.Day, workItemDeadline);
DailyWorkItemBacklog workItemBacklog = backlogManager.GetDailyBacklog(workItemDeadline);
Console.WriteLine($"Daily backlog WI count: {workItemBacklog.CurrentDay.Items.Count}");

WeeklyWorkItemBacklog weeklyBacklog = backlogManager.GetWeeklyBacklog(workItemDeadline);
Console.WriteLine($"Weekly backlog WI count: {weeklyBacklog.CurrentWeek.Items.Count}");

Console.Read();