using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public struct TamglyMonth : ITimeInterval
{
    public int Number { get; }
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TamglyMonth(DateOnly date)
    {
        var monthNumber = 1;
        var currentTime = TamglyTime.ZeroMonth;

        while (currentTime.AddMonths(1) < date)
        {
            currentTime = currentTime.AddMonths(1);
            monthNumber++;
        }

        Number = monthNumber;
        Start = currentTime;
        End = currentTime.AddMonths(1).AddDays(-1);
    }
}