using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.Projects;
using Tamgly.Integration.ExcelAdapter.Contracts;

namespace Tamgly.Integration.ExcelAdapter.Implementation;

public class WorkItemExcelElementSorter : IExcelElementSorter<WorkItemWithProjectAssociation>
{
    public IEnumerable<WorkItemWithProjectAssociation> Sort(IEnumerable<WorkItemWithProjectAssociation> elements)
    {
        return elements.OrderBy(e => e.WorkItem.Priority);
    }
}