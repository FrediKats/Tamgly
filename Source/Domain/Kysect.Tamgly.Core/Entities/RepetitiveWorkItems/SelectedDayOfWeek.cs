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
            DayOfWeek.Sunday => selectedDayOfWeek.HasFlag(DayOfWeek.Sunday),
            DayOfWeek.Monday => selectedDayOfWeek.HasFlag(DayOfWeek.Monday),
            DayOfWeek.Tuesday => selectedDayOfWeek.HasFlag(DayOfWeek.Tuesday),
            DayOfWeek.Wednesday => selectedDayOfWeek.HasFlag(DayOfWeek.Wednesday),
            DayOfWeek.Thursday => selectedDayOfWeek.HasFlag(DayOfWeek.Thursday),
            DayOfWeek.Friday => selectedDayOfWeek.HasFlag(DayOfWeek.Friday),
            DayOfWeek.Saturday => selectedDayOfWeek.HasFlag(DayOfWeek.Saturday),
            _ => throw new ArgumentOutOfRangeException(nameof(date.DayOfWeek))
        };
    }
}