using System.Collections.Generic;
using Tamgly.DataAccess.Models;

namespace Tamgly.DataAccess.Repositories;

public interface IWorkItemTrackingIntervalRepository
{
    IReadOnlyCollection<WorkItemTrackIntervalDatabaseRecord> GetForWorkItem(int workItemId);
}