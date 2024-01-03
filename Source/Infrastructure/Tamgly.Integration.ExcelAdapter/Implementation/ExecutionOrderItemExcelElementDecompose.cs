using ClosedXML.Excel;
using System;
using Tamgly.Integration.ExcelAdapter.Contracts;
using Tamgly.Integration.ExcelAdapter.Models;

namespace Tamgly.Integration.ExcelAdapter.Implementation;

public class ExecutionOrderItemExcelElementDecompose : IExcelElementDecomposer<WorkItemWithExecutionDate>
{
    public void SetupColumn(IXLWorksheet worksheet)
    {
        worksheet.Column(2).Width = 10;
        worksheet.Column(3).Width = 25;
    }

    public void AddTitle(IXLRow row)
    {
        row.Cell(1).Value = "Id";
        row.Cell(2).Value = "Date";
        row.Cell(3).Value = "Title";
        row.Cell(4).Value = "State";
        row.Cell(5).Value = "Estimate";
        row.Cell(6).Value = "Priority";
        row.Cell(7).Value = "Description";
    }

    public void Decompose(IXLRow row, WorkItemWithExecutionDate value)
    {
        row.Cell(1).Value = value.WorkItem.Id;
        row.Cell(2).Value = value.Date.ToDateTime(TimeOnly.MinValue);
        row.Cell(3).Value = value.WorkItem.Title;
        row.Cell(4).Value = value.WorkItem.State.ToString();
        row.Cell(5).Value = value.WorkItem.Estimate;
        row.Cell(6).Value = value.WorkItem.Priority.ToString();
        row.Cell(7).Value = value.WorkItem.Description;
    }
}