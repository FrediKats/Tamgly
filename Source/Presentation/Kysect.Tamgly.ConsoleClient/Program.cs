using Kysect.Tamgly.Core.Aggregates;
using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Entities.Backlogs;
using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Entities.TimeIntervals;
using Kysect.Tamgly.Core.Tools;

var workItemManager = new WorkItemManager();
var workItemBuilder = new WorkItemBuilder("Support projects");
WorkItem workItem = workItemBuilder.Build();
workItemManager.AddWorkItem(workItem);

var project = Project.Create("Tamgly");
workItemManager.AddProject(project);
workItemManager.ChangeProject(workItem, project);
Console.WriteLine($"Project WI count: {project.GetAllWorkItems().Count}");

var backlogManager = new BacklogManager(workItemManager);
DateOnly workItemDeadline = DateOnly.FromDateTime(DateTime.Today).AddDays(10);
workItem.Deadline = new WorkItemDeadline(new TamglyDay(workItemDeadline));
DailyWorkItemBacklog workItemBacklog = backlogManager.GetDailyBacklog(workItemDeadline);
Console.WriteLine($"Daily backlog WI count: {workItemBacklog.CurrentDay.Items.Count}");

workItem.Deadline = new WorkItemDeadline(new TamglyWeek(workItemDeadline));
WeeklyWorkItemBacklog weeklyBacklog = backlogManager.GetWeeklyBacklog(workItemDeadline);
Console.WriteLine($"Weekly backlog WI count: {weeklyBacklog.CurrentWeek.Items.Count}");

workItem.Deadline = new WorkItemDeadline(new TamglyMonth(workItemDeadline));
MonthlyWorkItemBacklog monthlyBacklog = backlogManager.GetMonthlyBacklog(workItemDeadline);
Console.WriteLine($"Monthly backlog WI count: {monthlyBacklog.CurrentMonth.Items.Count}");

Console.Read();