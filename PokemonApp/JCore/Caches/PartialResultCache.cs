using System.Collections.Generic;

namespace JCore.Caches
{
    public class PartialResultCache<T>
    {
        readonly Dictionary<string, Dictionary<string, IList<T>>> _cache;

        public PartialResultCache() => _cache = new Dictionary<string, Dictionary<string, IList<T>>>();

        public bool TryGetResult(string property, string term, T item, out bool result)
        {
            if (_cache.TryGetValue(property, out var propertyCache) && propertyCache.TryGetValue(term, out IList<T> cachedResults))
            {
                result = cachedResults.Contains(item);
                return true;
            }
            result = false;
            return false;
        }

        public void StoreResult(string property, string term, T item, bool result)
        {
            if (!result) return;
            if (!_cache.ContainsKey(property)) _cache[property] = new Dictionary<string, IList<T>>();
            if (!_cache[property].ContainsKey(term)) _cache[property][term] = new List<T>();
            _cache[property][term].Add(item);
        }
    }
}