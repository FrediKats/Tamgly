using System;

namespace Tamgly.RepetitiveEvents.Models;

public interface IRepetitiveEventPattern
{
    // TODO: enable C#11, move to net7
    //static abstract RepetitiveEventPatternType PatternType { get; }
    public RepetitiveEventPatternType PatternType { get; }

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }

    bool IsMatch(DateOnly value);
}