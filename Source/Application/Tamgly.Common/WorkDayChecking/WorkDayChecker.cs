using Kysect.CommonLib.DateAndTime;
using System;
using Tamgly.Common.DateAndTime;
using Tamgly.Common.Exceptions;

namespace Tamgly.Common.WorkDayChecking;

public class WorkDayChecker
{
    private readonly HolidayList _holidays;
    private readonly SelectedDayOfWeek _selectedDayOfWeek;

    public WorkDayChecker(HolidayList holidays, SelectedDayOfWeek selectedDayOfWeek)
    {
        if (selectedDayOfWeek == SelectedDayOfWeek.None)
            throw new TamglyException($"Cannot create Daily assignments because no work day selected");

        _holidays = holidays;
        _selectedDayOfWeek = selectedDayOfWeek;
    }

    public bool IsWorkDay(DateOnly dateOnly)
    {
        if (!_selectedDayOfWeek.Contains(dateOnly))
            return false;

        if (_holidays.IsHoliday(dateOnly))
            return false;

        return true;
    }

    public DateOnly GetNextWork(DateOnly from)
    {
        while (!IsWorkDay(from))
            from = from.AddDays(1);

        return from;
    }
}