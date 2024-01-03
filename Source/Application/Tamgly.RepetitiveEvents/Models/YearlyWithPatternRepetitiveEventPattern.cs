using Kysect.CommonLib.DateAndTime;
using System;

namespace Tamgly.RepetitiveEvents.Models;

public class YearlyWithPatternRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.YearlyWithPattern;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }
    public int WeekNumber { get; }
    public DayOfWeek DayOfWeek { get; }
    public Month Month { get; }

    public YearlyWithPatternRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval, int weekNumber, DayOfWeek dayOfWeek, Month month)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
        WeekNumber = weekNumber;
        DayOfWeek = dayOfWeek;
        Month = month;
    }

    public bool IsMatch(DateOnly value)
    {
        throw new NotImplementedException();
    }
}