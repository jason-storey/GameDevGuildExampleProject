using System.Collections.Generic;
using System.Linq;

namespace JasonStorey
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Paginated<T>(IEnumerable<T> items, int pageSize, int page = 1)
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
