using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.Projects;
using Tamgly.Integration.ExcelAdapter.Contracts;

namespace Tamgly.Integration.ExcelAdapter.Implementation;

public class WorkItemExcelWorkbookSeparatorByState : IExcelWorkbookSeparator<WorkItemWithProjectAssociation>
{
    public ILookup<string, WorkItemWithProjectAssociation> GroupToWorkbooks(IReadOnlyCollection<WorkItemWithProjectAssociation> elements)
    {
        return elements
            .OrderBy(r => r.Project?.Title ?? "Without")
            .ToLookup(
                w => w.Project?.Title ?? "Without",
                w => w);
    }
}