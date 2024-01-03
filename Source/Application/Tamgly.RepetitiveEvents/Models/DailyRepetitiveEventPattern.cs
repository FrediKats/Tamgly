using System;

namespace Tamgly.RepetitiveEvents.Models;

public class DailyRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.Daily;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }

    public DailyRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
    }

    public bool IsMatch(DateOnly value)
    {
        if (value < StartDate || EndDate < value)
            return false;

        int delta = value.DayNumber - StartDate.DayNumber;

        return delta % Interval == 0;
    }
}