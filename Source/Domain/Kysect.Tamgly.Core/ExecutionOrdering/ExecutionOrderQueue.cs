namespace Kysect.Tamgly.Core;

public class ExecutionOrderQueue
{
    private readonly Dictionary<WorkItemPriority, Queue<WorkItem>> _workItems;

    public ExecutionOrderQueue()
        : this(new Dictionary<WorkItemPriority, Queue<WorkItem>>())
    {
    }

    public ExecutionOrderQueue(Dictionary<WorkItemPriority, Queue<WorkItem>> workItems)
    {
        _workItems = workItems;
    }

    public void Add(WorkItem workItem)
    {
        if (workItem.Priority == null)
        {
            throw new TamglyException("Cannot add to execution order WI without priority");
        }

        GetOrCreateQueue(workItem.Priority.Value).Enqueue(workItem);
    }

    public bool Any()
    {
        foreach ((WorkItemPriority _, Queue<WorkItem>? value) in _workItems)
        {
            if (value.Any())
                return true;
        }

        return false;
    }

    public bool TryPeek(WorkItemPriority priority, out WorkItem? result)
    {
        return GetOrCreateQueue(priority).TryPeek(out result);
    }

    public WorkItem Dequeue(WorkItemPriority priority)
    {
        return _workItems[priority].Dequeue();
    }

    public bool IsAdded(WorkItem workItem)
    {
        return _workItems
            .SelectMany(wis => wis.Value)
            .Any(wi => wi.Id == workItem.Id);
    }

    private Queue<WorkItem> GetOrCreateQueue(WorkItemPriority priority)
    {
        if (!_workItems.ContainsKey(priority))
            _workItems[priority] = new Queue<WorkItem>();

        return _workItems[priority];
    }
}