using Kysect.Tamgly.Core.Entities.Deadlines;

namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public interface IRepetitiveInterval
{
    IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval();
}