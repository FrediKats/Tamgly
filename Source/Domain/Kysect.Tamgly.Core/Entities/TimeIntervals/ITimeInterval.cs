namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public interface ITimeInterval
{
    DateOnly Start { get; }
    DateOnly End { get; }
}