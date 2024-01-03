using System;
using Tamgly.Core.TimeIntervals;

namespace Tamgly.Core.Backlogs.Deadlines;

public class WorkItemDeadlineCreator
{
    public static WorkItemDeadline Create(WorkItemDeadlineType deadlineType, DateOnly? startTime)
    {
        ArgumentNullException.ThrowIfNull(startTime);

        return deadlineType switch
        {
            WorkItemDeadlineType.NoDeadline => new WorkItemDeadline(),
            WorkItemDeadlineType.Day => new WorkItemDeadline(new TamglyDay(startTime.Value)),
            WorkItemDeadlineType.Week => new WorkItemDeadline(new TamglyWeek(startTime.Value)),
            WorkItemDeadlineType.Month => new WorkItemDeadline(new TamglyMonth(startTime.Value)),
            _ => throw new ArgumentOutOfRangeException(nameof(deadlineType), deadlineType, null)
        };
    }
}