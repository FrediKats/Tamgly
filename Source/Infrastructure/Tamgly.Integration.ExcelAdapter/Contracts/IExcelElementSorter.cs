using System.Collections.Generic;

namespace Tamgly.Integration.ExcelAdapter.Contracts;

public interface IExcelElementSorter<T>
{
    IEnumerable<T> Sort(IEnumerable<T> elements);
}

public class ExcelElementSorterExtensions
{
    public static IExcelElementSorter<T> Empty<T>() => new ExcelElementNoSort<T>();
}

public class ExcelElementNoSort<T> : IExcelElementSorter<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> elements)
    {
        return elements;
    }
}