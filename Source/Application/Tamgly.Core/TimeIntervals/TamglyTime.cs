using System;
using Tamgly.Common.Exceptions;

namespace Tamgly.Core.TimeIntervals;

public static class TamglyTime
{
    public static readonly DateOnly ZeroDay = new DateOnly(2021, 12, 27);
    public static readonly DateOnly ZeroMonth = GetMonthStart(ZeroDay);

    public static DateOnly TodayDate => DateOnly.FromDateTime(DateTime.UtcNow);

    public static int DaysTo(this DateOnly first, DateOnly second)
    {
        TimeSpan interval = second.ToDateTime(TimeOnly.MinValue).Subtract(first.ToDateTime(TimeOnly.MinValue));
        return (int)interval.TotalDays;
    }

    public static DateOnly MaxOf(DateOnly first, DateOnly second)
    {
        return first >= second ? first : second;
    }

    public static DateOnly GetMonthStart(DateOnly date)
    {
        return date.AddDays(-date.Day).AddDays(1);
    }

    public static void EnsureDateIsSupported(DateOnly date)
    {
        if (date < ZeroDay)
            throw new TamglyException($"Only date after zero day is supported. Zero day: {ZeroDay}, date: {date}");
    }
}