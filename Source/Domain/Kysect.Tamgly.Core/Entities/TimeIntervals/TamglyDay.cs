using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public readonly struct TamglyDay : ITimeInterval
{
    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyDay(DateOnly start)
    {
        TamglyTime.EnsureDateIsSupported(start);

        Number = TamglyTime.ZeroDay.DaysTo(start);
        Start = start;
        End = start;
    }
}