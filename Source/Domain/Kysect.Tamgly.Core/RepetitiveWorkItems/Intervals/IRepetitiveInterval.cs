namespace Kysect.Tamgly.Core;

public interface IRepetitiveInterval
{
    IReadOnlyCollection<WorkItemDeadline> EnumeratePointOnInterval();
}