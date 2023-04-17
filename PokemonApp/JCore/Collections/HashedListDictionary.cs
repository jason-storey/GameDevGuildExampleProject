using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace JCore.Collections
{
    public class HashedListDictionary<T> : IListRepository<T>
    {
        readonly Dictionary<string, HashSet<T>> _elements;
        public HashedListDictionary() => _elements = new Dictionary<string, HashSet<T>>();

        public IEnumerable<T> GetValuesById(string key) => 
            TryGet(key, out var h) ? h : Enumerable.Empty<T>();

        bool TryGet(string key, out HashSet<T> h) => _elements.TryGetValue(key, out h);

        public int GetElementCount(string key) => TryGet(key, out var h) ? h.Count : 0;

        public bool AddElement(string key, T item)
        {
            if (TryGet(key, out var h))
                return h.Add(item);
            _elements[key] = new HashSet<T> { item };
            return true;
        }

        public bool RemoveElement(string key, T item) => TryGet(key, out var h) && h.Remove(item);

        public bool Update(string key, T item)
        {
            if (!TryGet(key, out var h))
                return false;
            h.Add(item);
            return true;
        }

        public bool Delete(string key, T item) => TryGet(key, out var h) && h.Remove(item);

        public bool DeleteAll(string key)
        {
            if (!TryGet(key, out var h)) return false;
            h.Clear();
            _elements.Remove(key);
            return true;
        }

        public void SetElements(string term, IEnumerable<T> items) => 
            _elements[term] = new HashSet<T>(items);
    }
}