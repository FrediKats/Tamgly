using System.Threading.Tasks;
using Tamgly.DataAccess;

namespace Tamgly.WorkItemStorage.Abstractions;

public interface IWorkItemStorage
{
    Task SyncWorkItems(TamglyDatabaseContext databaseContext);
}