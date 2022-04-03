namespace Kysect.Tamgly.Core.ValueObjects;

public struct TamglyWeek
{
    private static readonly DateTime ZeroDay = new DateTime(2021, 12, 27);

    private readonly int _weekNum;

    public DateTime Start => ZeroDay.AddDays(7 * _weekNum);
    public DateTime End => ZeroDay.AddDays(7 * (_weekNum +1)).AddTicks(-1);

    public TamglyWeek(int weekNum)
    {
        _weekNum = weekNum;
    }

    public static TamglyWeek FromDate(DateTime dateTime)
    {
        TimeSpan intervalFromZeroDay = dateTime.Subtract(ZeroDay);
        var weekCount = (int)(intervalFromZeroDay.TotalDays / 7);
        return new TamglyWeek(weekCount);
    }

    public bool Contains(DateTime dateTime)
    {
        return Start <= dateTime && dateTime <= End;
    }
}