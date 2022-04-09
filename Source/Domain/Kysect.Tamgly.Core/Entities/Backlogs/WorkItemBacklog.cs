using Kysect.Tamgly.Core.Entities.Deadlines;
using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.Backlogs;

public class WorkItemBacklog
{
    public WorkItemDeadline Deadline { get; }
    public ICollection<IWorkItem> Items { get; }

    public WorkItemBacklog(WorkItemDeadline deadline, ICollection<IWorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        Deadline = deadline;
        Items = items;

        if (items.Any(i => !i.Deadline.MatchedWith(Deadline)))
            throw new TamglyException("Try to create daily backlog with wrong deadline");
    }

    public static WorkItemBacklog Create(WorkItemDeadline deadline, IReadOnlyCollection<IWorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<IWorkItem> workItems = items
            .Where(i => i.Deadline.MatchedWith(deadline))
            .ToList();

        return new WorkItemBacklog(deadline, workItems);
    }

    public TimeSpan GetTotalEstimates()
    {
        TimeSpan result = TimeSpan.Zero;

        foreach (IWorkItem workItem in Items)
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