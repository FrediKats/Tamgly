﻿using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class WorkItemBacklog
{
    public WorkItemDeadline Deadline { get; }
    public ICollection<WorkItem> Items { get; }

    public static WorkItemBacklog Create(WorkItemDeadline deadline, IReadOnlyCollection<WorkItem> items)
    {
        List<WorkItem> workItems = items
            .Where(i => i.Deadline.MatchedWith(deadline))
            .ToList();

        return new WorkItemBacklog(deadline, workItems);
    }

    public WorkItemBacklog(WorkItemDeadline deadline, ICollection<WorkItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        Deadline = deadline;
        Items = items;

        if (items.Any(i => !i.Deadline.MatchedWith(Deadline)))
            throw new TamglyException("Try to create daily backlog with wrong deadline");
    }
}