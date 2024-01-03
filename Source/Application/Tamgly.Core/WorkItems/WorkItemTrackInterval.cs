using System;
using Tamgly.Common.Exceptions;

namespace Tamgly.Core.WorkItems;

public readonly struct WorkItemTrackInterval : IEquatable<WorkItemTrackInterval>
{
    public Guid Id { get; }
    public int ActivityId { get; }
    public DateTime StartTime { get; }
    public DateTime? EndTime { get; }

    public WorkItemTrackInterval(Guid id, int activityId, DateTime startTime, DateTime? endTime)
    {
        Id = id;
        ActivityId = activityId;
        StartTime = startTime;
        EndTime = endTime;

        if (EndTime is not null && EndTime < StartTime)
            throw new TamglyException($"Interval start is greater that end. Start: {StartTime}, end: {EndTime}");
    }

    public WorkItemTrackInterval ChangeTime(DateTime startTime, DateTime? endTime)
    {
        return new WorkItemTrackInterval(Id, ActivityId, startTime, endTime);
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