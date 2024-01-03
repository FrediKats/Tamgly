using System;
using System.Text.Json;
using Tamgly.RepetitiveEvents.Models;

namespace Tamgly.RepetitiveEvents.Tools;

public class RepetitiveEventPatternSerializer
{
    public static RepetitiveEventPatternSerializer Instance { get; } = new RepetitiveEventPatternSerializer();

    public string Serialize(IRepetitiveEventPattern value)
    {
        return JsonSerializer.Serialize(value);
    }

    public IRepetitiveEventPattern? Deserialize(string value, RepetitiveEventPatternType type)
    {
        return type switch
        {
            RepetitiveEventPatternType.Daily => JsonSerializer.Deserialize<DailyRepetitiveEventPattern>(value),
            RepetitiveEventPatternType.Weekly => JsonSerializer.Deserialize<WeeklyRepetitiveEventPattern>(value),
            RepetitiveEventPatternType.MonthlyWithDay => JsonSerializer.Deserialize<MonthlyWithDayRepetitiveEventPattern>(value),
            RepetitiveEventPatternType.MonthlyWithPattern => JsonSerializer.Deserialize<MonthlyWithDayRepetitiveEventPattern>(value),
            RepetitiveEventPatternType.YearlyWithDate => JsonSerializer.Deserialize<YearlyWithDateRepetitiveEventPattern>(value),
            RepetitiveEventPatternType.YearlyWithPattern => JsonSerializer.Deserialize<YearlyWithPatternRepetitiveEventPattern>(value),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}