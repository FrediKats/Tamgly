using Kysect.CommonLib.DateAndTime;
using System;

namespace Tamgly.RepetitiveEvents.Models;

public class WeeklyRepetitiveEventPattern : IRepetitiveEventPattern
{
    public RepetitiveEventPatternType PatternType => RepetitiveEventPatternType.Weekly;

    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public int Interval { get; }
    public SelectedDayOfWeek SelectedDayOfWeek { get; }

    public WeeklyRepetitiveEventPattern(DateOnly startDate, DateOnly endDate, int interval, SelectedDayOfWeek selectedDayOfWeek)
    {
        StartDate = startDate;
        EndDate = endDate;
        Interval = interval;
        SelectedDayOfWeek = selectedDayOfWeek;
    }

    public bool IsMatch(DateOnly value)
    {
        if (value < StartDate || EndDate < value)
            return false;

        throw new NotImplementedException();
    }
}