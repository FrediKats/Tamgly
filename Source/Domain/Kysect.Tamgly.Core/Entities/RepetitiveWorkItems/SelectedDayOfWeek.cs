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