namespace Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;

[Flags]
public enum SelectedDayOfWeek
{
    Monday = 2 << 0,
    Tuesday = 2 << 1,
    Wednesday = 2 << 2,
    Thursday = 2 << 3,
    Friday = 2 << 4,
    Saturday = 2 << 5,
    Sunday = 2 << 6,
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
}