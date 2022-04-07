namespace Kysect.Tamgly.Core.ValueObjects;

public readonly struct WorkItemDeadline : IEquatable<WorkItemDeadline>
{
    public enum Type
    {
        NoDeadline = 0,
        Day = 1,
        Week,
        Month,
    }

    private readonly Type _type;
    private readonly int? _value;

    public static WorkItemDeadline NoDeadline { get; } = new WorkItemDeadline(Type.NoDeadline, null);

    public static WorkItemDeadline Create(Type type, DateOnly dateTime)
    {
        switch (type)
        {
            case Type.Day:
                return new WorkItemDeadline(Type.Day, TamglyTime.ZeroDay.DaysTo(dateTime));

            case Type.Week:
                return new WorkItemDeadline(Type.Week, TamglyWeek.FromDate(dateTime).WeekNumber);

            case Type.Month:
                throw new NotImplementedException("Will be implemented later, in WI19");
            
            case Type.NoDeadline:
            default:
                throw new ArgumentException($"{type} is not acceptable type for deadline.");
        }
    }

    private WorkItemDeadline(Type type, int? value)
    {
        _type = type;
        _value = value;
    }

    public bool MatchedWith(Type type, DateOnly dateTime)
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