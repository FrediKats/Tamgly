using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using Tamgly.Core.Projects;
using Tamgly.Integration.ExcelAdapter.Contracts;

namespace Tamgly.Integration.ExcelAdapter.Implementation;

public class WorkItemExcelElementDecompose : IExcelElementDecomposer<WorkItemWithProjectAssociation>
{
    private readonly ILogger _logger;

    public WorkItemExcelElementDecompose(ILogger logger)
    {
        _logger = logger;
    }

    public void SetupColumn(IXLWorksheet worksheet)
    {
        worksheet.Column(2).Width = 15;
        worksheet.Column(6).Width = 35;
    }

    public void AddTitle(IXLRow row)
    {
        // TODO: custom column size for [Title]
        row.Cell(1).Value = "Id";
        row.Cell(2).Value = "Title";
        row.Cell(3).Value = "State";
        row.Cell(4).Value = "Estimate";
        row.Cell(5).Value = "Priority";
        row.Cell(6).Value = "Description";
    }

    public void Decompose(IXLRow row, WorkItemWithProjectAssociation value)
    {
        row.Cell(1).Value = value.WorkItem.Id;
        row.Cell(2).Value = value.WorkItem.Title;
        row.Cell(3).Value = value.WorkItem.State.ToString();
        row.Cell(4).Value = value.WorkItem.Estimate;
        row.Cell(5).Value = value.WorkItem.Priority.ToString();

        string? workItemDescription = value.WorkItem.Description;
        if (workItemDescription is not null && workItemDescription.Length > 100)
        {
            _logger.LogWarning($"Cannot add description for item {value.WorkItem.Title} ({value.WorkItem.Id}). Description length = {value.WorkItem.Description?.Length}");
            workItemDescription = "<long description was hidden>";
        }

        row.Cell(6).Value = workItemDescription;
    }
}