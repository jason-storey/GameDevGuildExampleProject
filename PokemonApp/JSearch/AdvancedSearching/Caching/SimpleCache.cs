using System.Collections.Generic;
using System.Linq;
using JCore.AdvancedSearch;

namespace JSearch
{
    public class SimpleCache<TKey, TValue> : ICache<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _cache;
        readonly int _capacity;
        public SimpleCache(int capacity = 100)
        {
            _cache = new Dictionary<TKey, TValue>(capacity);
            _capacity = capacity;
        }

        public bool TryGet(TKey key, out TValue value) => _cache.TryGetValue(key, out value);

        public void Add(TKey key, TValue value)
        {
            if (_cache.Count >= _capacity)
            {
                TKey firstKey = _cache.Keys.FirstOrDefault();
                if (firstKey != null)
                    _cache.Remove(firstKey);
            }
            _cache[key] = value;
        }
    }
}