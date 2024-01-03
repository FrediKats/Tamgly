using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamgly.Common.WorkDayChecking;

public class HolidayList
{
    private readonly IReadOnlyCollection<DateOnly> _holidays;

    public static HolidayList Create()
    {
        return new HolidayList(new List<DateOnly>
        {

        });
    }

    public HolidayList(IReadOnlyCollection<DateOnly> holidays)
    {
        _holidays = holidays;
    }

    public bool IsHoliday(DateOnly dateOnly)
    {
        return _holidays.Contains(dateOnly);
    }
}