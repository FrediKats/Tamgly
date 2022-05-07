using Kysect.Tamgly.Core;
using Spectre.Console.Cli;

namespace Kysect.Tamgly.ConsoleClient.Commands;

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