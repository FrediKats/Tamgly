namespace Kysect.Tamgly.Common;

[Flags]
public enum SelectedDayOfWeek
{
    None = 0,
    Monday = 1,
    Tuesday = 2 << 0,
    Wednesday = 2 << 1,
    Thursday = 2 << 2,
    Friday = 2 << 3,
    Saturday = 2 << 4,
    Sunday = 2 << 5,

    All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
}

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