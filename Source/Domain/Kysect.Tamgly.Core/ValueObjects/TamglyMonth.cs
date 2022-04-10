namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct TamglyMonth : IEquatable<TamglyMonth>
{
    public int MonthNumber { get; }

    public DateOnly Start => TamglyTime.ZeroMonth.AddMonths(MonthNumber);
    public DateOnly End => Start.AddMonths(1);

    public TamglyMonth(int monthNumber)
    {
        MonthNumber = monthNumber;
    }

    public static TamglyMonth FromDate(DateOnly dateTime)
    {
        var monthNumber = 0;
        var currentTime = TamglyTime.ZeroMonth;

        while (currentTime < dateTime)
        {
            currentTime = currentTime.AddMonths(1);
            monthNumber++;
        }

        return new TamglyMonth(monthNumber);
    }

    public bool Contains(DateOnly dateTime)
    {
        return Start <= dateTime && dateTime < End;
    }

    public bool Equals(TamglyMonth other)
    {
        return MonthNumber == other.MonthNumber;
    }

    public override bool Equals(object? obj)
    {
        return obj is TamglyMonth other && Equals(other);
    }

    public override int GetHashCode()
    {
        return MonthNumber;
    }
}