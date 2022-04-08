using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct TamglyDay
{
    public DateOnly Date { get; }
    public int Number { get; }

    public TamglyDay(DateOnly date)
    {
        TamglyTime.EnsureDateIsSupported(date);

        Date = date;
        Number = TamglyTime.ZeroDay.DaysTo(date);
    }
}