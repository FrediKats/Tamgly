using System.Collections.Generic;
using System.Linq;

namespace Tamgly.Integration.ExcelAdapter.Contracts;

public interface IExcelWorkbookSeparator<T>
{
    ILookup<string, T> GroupToWorkbooks(IReadOnlyCollection<T> elements);
}

public static class ExcelWorkbookSeparatorExtensions
{
    public static IExcelWorkbookSeparator<T> Default<T>()
    {
        return new DefaultExcelWorkbookSeparator<T>();
    }
}

public class DefaultExcelWorkbookSeparator<T> : IExcelWorkbookSeparator<T>
{
    public ILookup<string, T> GroupToWorkbooks(IReadOnlyCollection<T> elements)
    {
        return elements.ToLookup(
            _ => "Result",
            e => e);
    }
}