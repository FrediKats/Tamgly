using Microsoft.Extensions.Logging;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public static class TamglyEntityFrameworkDbContextExtensions
{
    public static TamglyDatabaseContext CreateContext(TamglyEntityFrameworkDbContext frameworkDbContext, ILogger logger)
    {
        return new TamglyDatabaseContext(
            new WorkItemRepository(frameworkDbContext),
            new ProjectRepository(frameworkDbContext, logger),
            new RepetitiveWorkItemConfigurationRepository(frameworkDbContext),
            new WorkItemTrackingIntervalRepository(frameworkDbContext));
    }
}