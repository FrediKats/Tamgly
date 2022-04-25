namespace Kysect.Tamgly.Core;

public class WorkItemBacklog
{
    public WorkItemDeadline Deadline { get; }
    public ICollection<WorkItem> Items { get; }

    public WorkItemBacklog(WorkItemDeadline deadline, ICollection<WorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        Deadline = deadline;
        Items = items;

        if (items.Any(i => !i.Deadline.MatchedWith(Deadline)))
            throw new TamglyException("Try to create daily backlog with wrong deadline");
    }

    public static WorkItemBacklog Create(WorkItemDeadline deadline, IReadOnlyCollection<WorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<WorkItem> workItems = items
            .Where(i => i.Deadline.MatchedWith(deadline))
            .ToList();

        return new WorkItemBacklog(deadline, workItems);
    }

    public TimeSpan GetTotalEstimates()
    {
        TimeSpan result = TimeSpan.Zero;

        foreach (WorkItem workItem in Items)
        {
            if (workItem.Estimate != null)
            {
                result = result.Add(workItem.Estimate.Value);
            }
        }

        return result;
    }

    public TimeSpan? GetAverageDailyEstimate()
    {
        int daysBeforeDeadlineCount = Deadline.GetDaysBeforeDeadlineCount();
        if (daysBeforeDeadlineCount == 0)
            return null;

        return GetTotalEstimates() / daysBeforeDeadlineCount;
    }
}