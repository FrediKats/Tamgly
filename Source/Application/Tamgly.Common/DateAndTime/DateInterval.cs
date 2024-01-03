using System;
using System.Collections.Generic;

namespace Tamgly.Common.DateAndTime;

public class DateInterval
{
    public static IEnumerable<DateOnly> GetDatesOnInterval(DateOnly from, DateOnly to)
    {
        DateOnly current = from;
        while (current < to)
        {
            yield return current;
            current = current.AddDays(1);
        }
    }
}