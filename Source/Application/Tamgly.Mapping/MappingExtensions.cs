using System.Collections.Generic;
using System.Linq;

namespace Tamgly.Mapping;

public static class MappingExtensions
{
    public static IReadOnlyCollection<TSecond> Map<TFirst, TSecond>(this IMapper<TFirst, TSecond> mapper, IEnumerable<TFirst> values)
    {
        return values
            .Select(mapper.Map)
            .ToList();
    }

    public static IReadOnlyCollection<TFirst> Map<TFirst, TSecond>(this IMapper<TFirst, TSecond> mapper, IEnumerable<TSecond> values)
    {
        return values
            .Select(mapper.Map)
            .ToList();
    }
}