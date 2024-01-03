using System;

namespace Tamgly.RepetitiveEvents.Models;

public class MonthlyWithDayRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.MonthlyWithDay;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }
    public int DayOfMonth { get; }

    public MonthlyWithDayRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval, int dayOfMonth)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
        DayOfMonth = dayOfMonth;
    }

    public bool IsMatch(DateOnly value)
    {
        if (value < StartDate || EndDate < value)
            return false;

        if (value.DayNumber != DayOfMonth)
            return false;

        // TODO: optimize
        DateOnly current = StartDate;
        while (current <= value)
        {
            if (current.Year == value.Year && current.Month == value.Month)
                return true;

            current = current.AddMonths(Interval);
        }

        return false;
    }
}