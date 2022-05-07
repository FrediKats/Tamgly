using Kysect.Tamgly.Common;

namespace Kysect.Tamgly.Core;

public struct WorkingHours
{
    public static WorkingHours Empty { get; } = new WorkingHours(null, null, null, null);

    public TimeSpan? PerDay { get; }
    public SelectedDayOfWeek? SelectedDays { get; }
    public TimeSpan? PerWeek { get; }
    public TimeSpan? PerMonth { get; }

    public WorkingHours(TimeSpan? perDay, SelectedDayOfWeek? selectedDays, TimeSpan? perWeek, TimeSpan? perMonth)
    {
        PerDay = perDay;
        SelectedDays = selectedDays;
        PerWeek = perWeek;
        PerMonth = perMonth;
    }

    public WorkingHoursWarning? ValidateDailyLimit(TamglyDay day, IReadOnlyCollection<WorkItem> workItems)
    {
        TimeSpan? totalEstimates = workItems.Select(wi => wi.Estimate).Sum();
        if (totalEstimates is null || totalEstimates.Value == TimeSpan.Zero)
            return null;

        if (SelectedDays is not null
            && !SelectedDays.Value.Contains(day.Value))
        {
            return new WorkingHoursWarning(day, TimeSpan.Zero, totalEstimates.Value);
        }

        if (PerDay is not null
            && PerDay.Value < totalEstimates.Value)
        {
            return new WorkingHoursWarning(day, PerDay.Value, totalEstimates.Value);
        }

        return null;
    }

    public WorkingHoursWarning? ValidateWeeklyLimit(TamglyWeek week, IReadOnlyCollection<WorkItem> workItems)
    {
        TimeSpan? totalEstimates = workItems.Select(wi => wi.Estimate).Sum();
        if (totalEstimates is null || totalEstimates.Value == TimeSpan.Zero)
            return null;

        if (PerWeek is not null
            && PerWeek.Value < totalEstimates.Value)
        {
            return new WorkingHoursWarning(week, PerWeek.Value, totalEstimates.Value);
        }

        return null;
    }

    public WorkingHoursWarning? ValidateMonthlyLimit(TamglyMonth month, IReadOnlyCollection<WorkItem> workItems)
    {
        TimeSpan? totalEstimates = workItems.Select(wi => wi.Estimate).Sum();
        if (totalEstimates is null || totalEstimates.Value == TimeSpan.Zero)
            return null;

        if (PerMonth is not null
            && PerMonth.Value < totalEstimates.Value)
        {
            return new WorkingHoursWarning(month, PerMonth.Value, totalEstimates.Value);
        }

        return null;
    }
}