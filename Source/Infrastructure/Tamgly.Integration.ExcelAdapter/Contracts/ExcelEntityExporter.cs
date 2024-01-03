using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tamgly.Integration.ExcelAdapter.Contracts;

public class ExcelEntityExporter<T>
{
    private readonly bool _removePreviousFile;
    private readonly string _worksheetName;
    private readonly IExcelElementDecomposer<T> _elementDecomposer;
    private readonly IExcelWorkbookSeparator<T> _separator;
    private readonly IExcelElementSorter<T> _sorter;

    public ExcelEntityExporter(
        string worksheetName,
        bool removePreviousFile,
        IExcelElementDecomposer<T> elementDecomposer,
        IExcelWorkbookSeparator<T> separator,
        IExcelElementSorter<T> sorter)
    {
        _removePreviousFile = removePreviousFile;
        _worksheetName = worksheetName;
        _elementDecomposer = elementDecomposer;
        _separator = separator;
        _sorter = sorter;

        if (_removePreviousFile)
            File.Delete(_worksheetName);
    }

    public void Export(IReadOnlyList<T> values)
    {
        using var workbook = new XLWorkbook();

        ILookup<string, T> groupToWorkbooks = _separator.GroupToWorkbooks(values);

        foreach (IGrouping<string, T> groupToWorkbook in groupToWorkbooks)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add(groupToWorkbook.Key);
            var currentWorksheetElements = _sorter.Sort(groupToWorkbook).ToList();

            _elementDecomposer.SetupColumn(worksheet);
            _elementDecomposer.AddTitle(worksheet.Row(1));
            for (int i = 0; i < currentWorksheetElements.Count; i++)
                _elementDecomposer.Decompose(worksheet.Row(i + 2), currentWorksheetElements[i]);
        }

        workbook.SaveAs(_worksheetName);
    }
}