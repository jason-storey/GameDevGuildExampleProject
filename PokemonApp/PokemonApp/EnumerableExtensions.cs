using System.Collections.Generic;
using System.Linq;

namespace PokemonApp
{
    public static class EnumerableExtensions
    {
        public const int DEFAULT_PAGE_SIZE = 20;

        public static IEnumerable<T> Paginated<T>(this IEnumerable<T> items,int pageSize, int page = 1)
        {
            if (items == null) return Enumerable.Empty<T>();
            if (page <= 0) return items;
            if (pageSize <= 0) pageSize = DEFAULT_PAGE_SIZE;
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}