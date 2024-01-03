using System.Collections.Generic;
using System.Linq;
using Tamgly.DataAccess.Models;
using Tamgly.DataAccess.Repositories;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public class WorkItemTrackingIntervalRepository : IWorkItemTrackingIntervalRepository
{
    private readonly TamglyEntityFrameworkDbContext _context;

    public WorkItemTrackingIntervalRepository(TamglyEntityFrameworkDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<WorkItemTrackIntervalDatabaseRecord> GetForWorkItem(int workItemId)
    {
        return _context.WorkItemTrackIntervals
            .Where(i => i.ActivityId == workItemId)
            .ToList();
    }
}