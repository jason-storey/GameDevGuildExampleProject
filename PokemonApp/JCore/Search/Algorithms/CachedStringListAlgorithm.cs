using System;
using System.Collections.Generic;
using System.Linq;
using JCore.Search;

namespace JCore.Search
{
    public abstract class CachedStringListAlgorithm : ISearchAlgorithm
    {
        readonly IListRepository<string> _cache;
        readonly Func<string, IEnumerable<string>> _algorithm;

        protected CachedStringListAlgorithm(IListRepository<string> cache,Func<string,IEnumerable<string>> algorithm)
        {
            _cache = cache;
            _algorithm = algorithm;
        }
        public bool IsMatch(string searchTerm, string content)
        {
            var first = Get(searchTerm);
            var second = Get(content);
            return first.Any(second.Contains);
        }
        
        IEnumerable<string> Get(string term)
        {
            var elements = _cache.GetValuesById(term).ToList();
            if (elements.Count > 0) return elements;
            elements.AddRange(_algorithm.Invoke(term));
            _cache.SetElements(term,elements);
            return elements;
        }
    }
}