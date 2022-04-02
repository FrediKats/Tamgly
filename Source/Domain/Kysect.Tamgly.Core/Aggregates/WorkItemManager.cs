using Kysect.Tamgly.Core.Entities;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Aggregates;

public class WorkItemManager
{
    private readonly List<WorkItem> _items;

    public WorkItemManager() : this(new List<WorkItem>())
    {
    }

    public WorkItemManager(List<WorkItem> items)
    {
        _items = items;
    }

    public void AddWorkItem(WorkItem item)
    {
        _items.Add(item);
    }

    public void RemoveWorkItem(WorkItem item)
    {
        if (!_items.Remove(item))
        {
            throw new TamglyException($"Work item with id {item.Id} was not found.");
        }
    }

    public IReadOnlyCollection<WorkItem> GetWorkItems()
    {
        return _items;
    }
}