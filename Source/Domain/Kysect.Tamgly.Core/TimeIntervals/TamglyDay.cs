namespace Kysect.Tamgly.Core;

public readonly struct TamglyDay : IEquatable<TamglyDay>, ITimeInterval
{
    public int Number { get; }
    public DateOnly Value { get; }

    DateOnly ITimeInterval.Start => Value;
    DateOnly ITimeInterval.End => Value;

    public TamglyDay(DateOnly start)
    {
        TamglyTime.EnsureDateIsSupported(start);

        Number = TamglyTime.ZeroDay.DaysTo(start);
        Value = start;
    }

    public static IEnumerable<TamglyDay> EnumerateDays(ITimeInterval interval, int period = 1)
    {
        for (var current = new TamglyDay(interval.Start); current.Value < interval.End; current = current.AddDays(period))
            yield return current;
    }

    public TamglyDay AddDays(int count = 1)
    {
        return new TamglyDay(Value.AddDays(count));
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