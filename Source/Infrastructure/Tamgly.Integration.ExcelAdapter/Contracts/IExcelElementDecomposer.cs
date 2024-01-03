using ClosedXML.Excel;

namespace Tamgly.Integration.ExcelAdapter.Contracts;

public interface IExcelElementDecomposer<T>
{
    void SetupColumn(IXLWorksheet worksheet);
    void AddTitle(IXLRow row);
    void Decompose(IXLRow row, T value);
}