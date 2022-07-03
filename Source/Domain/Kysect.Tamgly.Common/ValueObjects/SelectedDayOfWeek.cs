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