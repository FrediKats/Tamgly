namespace Kysect.Tamgly.Common;

public static class SelectedDayOfWeekExtensions
{
    public static bool Contains(this SelectedDayOfWeek selectedDayOfWeek, DateOnly date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Monday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Monday),
            DayOfWeek.Tuesday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Tuesday),
            DayOfWeek.Wednesday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Wednesday),
            DayOfWeek.Thursday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Thursday),
            DayOfWeek.Friday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Friday),
            DayOfWeek.Saturday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Saturday),
            DayOfWeek.Sunday => selectedDayOfWeek.HasFlag(SelectedDayOfWeek.Sunday),
            _ => throw new ArgumentOutOfRangeException($"Invalid day of week for {date}: {date.DayOfWeek}")
        };
    }

    public static DateOnly NextDayInRange(this DateOnly date, SelectedDayOfWeek selectedDayOfWeek)
    {
        while (!selectedDayOfWeek.Contains(date))
            date = date.AddDays(1);

        return date;
    }
}