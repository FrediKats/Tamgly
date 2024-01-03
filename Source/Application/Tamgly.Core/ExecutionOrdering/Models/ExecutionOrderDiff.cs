using System;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.ExecutionOrdering.Models;

public record struct ExecutionOrderDiff(WorkItem WorkItem, DateOnly Before, DateOnly After);