using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities;

public class DailyBacklog
{
    public DateTime Deadline { get; }
    public ICollection<WorkItem> Items { get; }

    public DailyBacklog(DateTime deadline, ICollection<WorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        Items = items;
        Deadline = deadline;

        if (items.Any(i => i.Deadline is null || i.Deadline.Value != Deadline))
            throw new TamglyException("Try to create daily backlog with wrong deadline");
    }
}