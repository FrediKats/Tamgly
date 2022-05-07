namespace Kysect.Tamgly.Core;

public interface ITimeInterval
{
    DateOnly Start { get; }
    DateOnly End { get; }
}