using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.WorkItems;

namespace Tamgly.WorkItemStorage.Abstractions;

public record WorkItemStorageSyncResult(WorkItemStorageImportResult Imported, IReadOnlyCollection<WorkItem> Updated);

public record WorkItemStorageImportResult(IReadOnlyCollection<WorkItem> WorkItemsWithoutProject, ILookup<string, WorkItem> ProjectWorkItems);