using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities.TimeIntervals;

public readonly struct TamglyMonth : IEquatable<TamglyMonth>, ITimeInterval
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

    public bool Equals(TamglyMonth other)
    {
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        return obj is TamglyMonth other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}