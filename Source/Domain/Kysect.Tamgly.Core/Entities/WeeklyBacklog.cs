using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WeeklyBacklog
{
    public TamglyWeek Deadline { get; }
    public ICollection<WorkItem> Items { get; }

    public WeeklyBacklog(TamglyWeek deadline, ICollection<WorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        Items = items;
        Deadline = deadline;

        if (items.Any(i => i.Deadline is not null && !Deadline.Contains(i.Deadline.Value)))
            throw new TamglyException("Try to create daily backlog with wrong deadline");
    }
}