using System;

namespace Tamgly.RepetitiveEvents.Models;

public class MonthlyWithPatternRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.MonthlyWithPattern;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }
    public int WeekNumber { get; }
    public DayOfWeek DayOfWeek { get; }

    public MonthlyWithPatternRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval, int weekNumber, DayOfWeek dayOfWeek)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
        WeekNumber = weekNumber;
        DayOfWeek = dayOfWeek;
    }

    public bool IsMatch(DateOnly value)
    {
        // TODO: implement
        throw new NotImplementedException();
    }
}