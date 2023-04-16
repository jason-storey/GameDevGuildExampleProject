using System.Collections.Generic;
using System.Linq;

namespace PokemonApp
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Paginated<T>(this IEnumerable<T> items,int pageSize, int page = 1) => 
            items.Skip((page - 1) * pageSize).Take(pageSize);
    }
}