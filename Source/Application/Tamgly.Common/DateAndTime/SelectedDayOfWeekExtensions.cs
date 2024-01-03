using Kysect.CommonLib.DateAndTime;
using System;

namespace Tamgly.Common.DateAndTime;

public static class SelectedDayOfWeekExtensions
{
    public static bool Contains(this SelectedDayOfWeek selectedDayOfWeek, DateOnly date)
    {
        return selectedDayOfWeek.Contains(date.DayOfWeek);
    }

    public static DateOnly NextDayInRange(this DateOnly date, SelectedDayOfWeek selectedDayOfWeek)
    {
        while (!selectedDayOfWeek.Contains(date))
            date = date.AddDays(1);

        return date;
    }
}