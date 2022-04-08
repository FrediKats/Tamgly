namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

public interface IRepetitiveInterval
{
    IReadOnlyCollection<DateOnly> EnumeratePointOnInterval();
}