using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using Tamgly.Core.Aggregates;
using Tamgly.Core.WorkItems;

namespace Tamgly.ConsoleClient.Commands;

public class GetWorkItemCommand : Command<GetWorkItemCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    private readonly WorkItemManager _itemManager;

    public GetWorkItemCommand(WorkItemManager itemManager)
    {
        _itemManager = itemManager;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        IReadOnlyCollection<WorkItem> items = _itemManager.GetSelfWorkItems();

        foreach (WorkItem workItem in items)
            Console.WriteLine(workItem.ToShortString());
        return 0;
    }
}