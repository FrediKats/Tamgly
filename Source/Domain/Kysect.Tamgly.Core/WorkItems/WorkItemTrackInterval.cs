using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities;

public readonly struct WorkItemTrackInterval : IEquatable<WorkItemTrackInterval>
{
    public Guid Id { get; }
    public DateTime StartTime { get; }
    public DateTime? EndTime { get; }

    public WorkItemTrackInterval(Guid id, DateTime startTime, DateTime? endTime)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;

        if (EndTime is not null && EndTime < StartTime)
            throw new TamglyException($"Interval start is greater that end. Start: {StartTime}, end: {EndTime}");
    }

    public WorkItemTrackInterval ChangeTime(DateTime startTime, DateTime? endTime)
    {
        return new WorkItemTrackInterval(Id, startTime, endTime);
    }

    public TimeSpan? GetDuration()
    {
        if (EndTime is null)
            return null;

        return EndTime.Value.Subtract(StartTime);
    }

    public bool Equals(WorkItemTrackInterval other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is WorkItemTrackInterval other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}