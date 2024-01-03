using Kysect.CommonLib.DateAndTime;
using System;

namespace Tamgly.RepetitiveEvents.Models;

public class YearlyWithDateRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.YearlyWithDate;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }
    public int DayOfMonth { get; }
    public Month Month { get; }

    public YearlyWithDateRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval, int dayOfMonth, Month month)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
        DayOfMonth = dayOfMonth;
        Month = month;
    }

    public bool IsMatch(DateOnly value)
    {
        throw new NotImplementedException();
    }
}