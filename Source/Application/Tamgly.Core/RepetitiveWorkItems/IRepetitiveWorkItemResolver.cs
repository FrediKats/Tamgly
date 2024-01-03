using System.Collections.Generic;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.RepetitiveWorkItems;

public interface IRepetitiveWorkItemResolver
{
    IReadOnlyCollection<WorkItem> GetRepetitiveItems(WorkItem workItem);
}