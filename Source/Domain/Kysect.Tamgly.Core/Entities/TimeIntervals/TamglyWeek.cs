using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public readonly struct TamglyWeek : ITimeInterval
{
    public const int DayInWeek = 7;

    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyWeek(DateOnly date)
    {
        TamglyTime.EnsureDateIsSupported(date);

        int weekNumber = TamglyTime.ZeroDay.DaysTo(date) / DayInWeek;

        Number = weekNumber;
        Start = TamglyTime.ZeroDay.AddDays(DayInWeek * Number);
        End = Start.AddDays(DayInWeek).AddDays(-1);
    }

    public TamglyWeek AddWeek(int count = 1)
    {
        return new TamglyWeek(Start.AddDays(DayInWeek * count));
    }

    public IEnumerable<DateOnly> EnumerateDays()
    {
        for (int i = 0; i < DayInWeek; i++)
        {
            yield return Start.AddDays(i);
        }
    }

}