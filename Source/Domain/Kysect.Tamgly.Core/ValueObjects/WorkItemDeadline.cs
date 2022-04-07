namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct WorkItemDeadline : IEquatable<WorkItemDeadline>
{
    private readonly WorkItemDeadlineType _type;
    private readonly int? _value;

    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline(WorkItemDeadlineType.NoDeadline, null);

    public static WorkItemDeadline Create(WorkItemDeadlineType type, DateOnly dateTime)
    {
        switch (type)
        {
            case WorkItemDeadlineType.Day:
                return new WorkItemDeadline(WorkItemDeadlineType.Day, TamglyTime.ZeroDay.DaysTo(dateTime));

            case WorkItemDeadlineType.Week:
                return new WorkItemDeadline(WorkItemDeadlineType.Week, TamglyWeek.FromDate(dateTime).WeekNumber);

            case WorkItemDeadlineType.Month:
                throw new NotImplementedException("Will be implemented later, in WI19");
            
            case WorkItemDeadlineType.NoDeadline:
            default:
                throw new ArgumentException($"{type} is not acceptable type for deadline.");
        }
    }

    private WorkItemDeadline(WorkItemDeadlineType type, int? value)
    {
        _type = type;
        _value = value;
    }

    public bool MatchedWith(WorkItemDeadlineType type, DateOnly dateTime)
    {
        WorkItemDeadline other = Create(type, dateTime);
        return Equals(other);
    }

    public bool MatchedWith(WorkItemDeadline other)
    {
        return Equals(other);
    }

    public bool Equals(WorkItemDeadline other)
    {
        return _type == other._type && _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is WorkItemDeadline other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)_type, _value);
    }
}