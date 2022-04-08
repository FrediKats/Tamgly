using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public interface IWorkItemDeadline
{
    WorkItemDeadlineType DeadlineType { get; }
    DateOnly Start { get; }

    /// <summary>
    /// End day is not included
    /// </summary>
    DateOnly End { get; }

    bool Contains(DateOnly dateTime)
    {
        return Start <= dateTime && dateTime < End;
    }
}