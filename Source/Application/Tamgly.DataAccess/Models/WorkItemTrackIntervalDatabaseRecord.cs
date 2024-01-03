using System;
using System.ComponentModel.DataAnnotations;

namespace Tamgly.DataAccess.Models;

public class WorkItemTrackIntervalDatabaseRecord
{
    [Key]
    public Guid Id { get; init; }
    public int ActivityId { get; init; }
    public DateTime StartTime { get; }
    public DateTime? EndTime { get; }

    public WorkItemTrackIntervalDatabaseRecord(Guid id, int activityId, DateTime startTime, DateTime? endTime)
        : this()
    {
        Id = id;
        ActivityId = activityId;
        StartTime = startTime;
        EndTime = endTime;
    }

    protected WorkItemTrackIntervalDatabaseRecord()
    {
    }
}