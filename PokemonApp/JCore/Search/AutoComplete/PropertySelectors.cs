using System;
using System.Collections.Generic;
using System.Linq;

namespace JCore.Search
{
    public class PropertySelectors<T>
    {
        readonly List<Func<T, string>> _selectors;
        public PropertySelectors() => _selectors = new List<Func<T, string>>();
        public void Add(params Func<T, string>[] selector) => _selectors.AddRange(selector);

        public IEnumerable<string> Select(T  item) => 
            _selectors.Select(selector => selector?.Invoke(item));
    }
}