namespace Kysect.Tamgly.Core;

public readonly struct TamglyDay : IEquatable<TamglyDay>, ITimeInterval
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

    public bool Equals(TamglyDay other)
    {
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        return obj is TamglyDay other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}