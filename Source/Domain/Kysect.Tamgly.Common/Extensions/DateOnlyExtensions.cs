﻿namespace Kysect.Tamgly.Common;

public static class DateOnlyExtensions
{
    public static DateOnly Max(DateOnly a, DateOnly b)
    {
        return a > b ? a : b;
    }

    public static DateOnly Min(DateOnly a, DateOnly b)
    {
        return a < b ? a : b;
    }
}