using Tamgly.ConsoleClient.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using Tamgly.Core.Aggregates;
using Tamgly.Core.Backlogs.Deadlines;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;
using Kysect.CommonLib.Exceptions;
using Tamgly.Common.IdentifierGenerators;

namespace Tamgly.ConsoleClient.Commands;

public class CreateWorkItemCommand : Command<CreateWorkItemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Title]")]
        public string Title { get; set; }
        [CommandArgument(1, "[Description]")]
        public string Description { get; set; }
        [CommandArgument(2, "[Estimate]")]
        public TimeSpan? Estimate { get; set; }
        [CommandArgument(3, "[DeadlineType]")]
        public DeadlineInputType? DeadlineType { get; set; }
        [CommandArgument(4, "[Deadline]")]
        public DateOnly? Deadline { get; set; }
        [CommandArgument(5, "[Priority]")]
        public WorkItemPriority? Priority { get; set; }
    }

    private readonly WorkItemManager _itemManager;
    private readonly IIdentifierGenerator _identifierGenerator;

    public CreateWorkItemCommand(WorkItemManager itemManager, IIdentifierGenerator identifierGenerator)
    {
        _itemManager = itemManager;
        _identifierGenerator = identifierGenerator;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (settings.Title is null)
            return ValidationResult.Error("Title is missed.");

        if (settings.DeadlineType is not null
            && settings.DeadlineType != DeadlineInputType.No
            && settings.Deadline is null)
        {
            return ValidationResult.Error("Deadline is missed.");
        }

        if ((settings.DeadlineType is null || settings.DeadlineType == DeadlineInputType.No)
            && settings.Deadline is not null)
        {
            return ValidationResult.Error("Deadline type is invalid.");
        }

        return base.Validate(context, settings);
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        WorkItemDeadline deadline = WorkItemDeadline.NoDeadline;
        DeadlineInputType? deadlineInputType = settings.DeadlineType;
        if (deadlineInputType.HasValue && deadlineInputType != 0)
        {
            if (settings.Deadline is null)
                throw new ArgumentException("Deadline is missed");

            switch (deadlineInputType)
            {
                case DeadlineInputType.No:
                    break;
                case DeadlineInputType.Day:
                    deadline = new WorkItemDeadline(new TamglyDay(settings.Deadline.Value));
                    break;
                case DeadlineInputType.Week:
                    deadline = new WorkItemDeadline(new TamglyWeek(settings.Deadline.Value));
                    break;
                case DeadlineInputType.Month:
                    deadline = new WorkItemDeadline(new TamglyMonth(settings.Deadline.Value));
                    break;
                default:
                    throw SwitchDefaultExceptions.OnUnexpectedValue(deadlineInputType);
            }
        }

        WorkItemBuilder workItemBuilder = new WorkItemBuilder(settings.Title, _identifierGenerator)
            .SetDescription(settings.Description)
            .SetEstimates(settings.Estimate)
            .SetDeadline(deadline)
            .SetPriority(settings.Priority);

        WorkItem workItem = workItemBuilder.Build();
        _itemManager.AddWorkItem(workItem);

        return 0;
    }
}