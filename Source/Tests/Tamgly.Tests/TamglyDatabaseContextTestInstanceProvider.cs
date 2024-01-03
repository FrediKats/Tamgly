using Microsoft.Extensions.Logging;
using Tamgly.DataAccess;
using Tamgly.DataAccess.EntityFrameworkAdapter;

namespace Tamgly.Tests;

public static class TamglyDatabaseContextTestInstanceProvider
{
    public static TamglyDatabaseContext Provider(ILogger logger)
    {
        return TamglyEntityFrameworkDbContextExtensions.CreateContext(new TamglyEntityFrameworkDbContext(), logger);
    }
}