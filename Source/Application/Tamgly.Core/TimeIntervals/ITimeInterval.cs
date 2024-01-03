using System;

namespace Tamgly.Core.TimeIntervals;

public interface ITimeInterval
{
    DateOnly Start { get; }
    DateOnly End { get; }
}