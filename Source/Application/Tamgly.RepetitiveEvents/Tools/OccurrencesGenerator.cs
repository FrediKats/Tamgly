using System;
using System.Collections.Generic;
using Tamgly.Common.DateAndTime;
using Tamgly.RepetitiveEvents.Models;

namespace Tamgly.RepetitiveEvents.Tools;

public class OccurrenceGenerator
{
    public static OccurrenceGenerator Instance { get; } = new OccurrenceGenerator();

    public IReadOnlyCollection<DateOnly> GetOccurrences(IRepetitiveEventPattern pattern, DateOnly start, DateOnly end)
    {
        var result = new List<DateOnly>();

        foreach (DateOnly value in DateInterval.GetDatesOnInterval(start, end))
        {
            if (pattern.IsMatch(value))
                result.Add(value);
        }

        return result;
    }
}