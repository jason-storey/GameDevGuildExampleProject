using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JCore.Search;

namespace JCore.Collections
{
    public class Index<T> : IRepository<T>
    {
        readonly Dictionary<string, T> _index;
        readonly Func<T, string> _createKey;

        public Index(Dictionary<string,T> dictionary) : this(dictionary,CreateKeyFromHash) { }

        static string CreateKeyFromHash(T item) => item.GetHashCode().ToString();

        public Index(Dictionary<string,T> dictionary,Func<T,string> createKey)
        {
            _index = dictionary;
            _createKey = createKey;
        }

        public Index(bool caseSensitive = false) => _index = new Dictionary<string, T>(caseSensitive ? StringComparer.Ordinal :  StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, T> ToDictionary() => _index;

        public void Add(T item, Func<T, string> key)
        {
            var itemKey = key?.Invoke(item);
            if (string.IsNullOrWhiteSpace(itemKey)) return;
            _index[itemKey] = item;
        }

        public void Add(T item, string key) => _index[key] = item;

        public IEnumerable<T> Search(string searchTerm, ISearchAlgorithm algorithm)
        {
            var matches = _index.Keys.Where(x => algorithm.IsMatch(searchTerm, x));
            foreach (var match in matches)
                yield return _index[match];
        }
        
        public void AddAll(Dictionary<string, T> items)
        {
            foreach (var item in items) 
                _index.Add(item.Key,item.Value);
        }

        public bool Has(string key) => _index.ContainsKey(key);
        public IEnumerable<string> Keys => _index.Keys;

        public T this[string key] => TryGet(key, out var item) ? item : default;
        
        bool TryGet(string key, out T item) => _index.TryGetValue(key, out item);

        public static Index<T> From(Dictionary<string, T> dict,bool caseSensitive = false)
        {
            var index = new Index<T>(caseSensitive);
            foreach (var kvp in dict) 
                index.Add(kvp.Value, kvp.Key);
            return index;
        }
        public static Index<T> From(IEnumerable<T> items, Func<T, string> key,bool caseSensitive = false)
        {
            var index = new Index<T>(caseSensitive);
            foreach (var item in items) index.Add(item,key);
            return index;
        }

        public static implicit operator Index<T>(Dictionary<string, T> items) => new Index<T>(items);
        public static implicit operator Dictionary<string, T>(Index<T> index) => index.ToDictionary();
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_index.Values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public T GetById(string key) => this[key];
        public IEnumerable<T> GetAll() => _index.Values;
        public void Update(string key, T item) => _index[key] = item;
        public void Delete(string key) => _index.Remove(key);
        public void Add(T item)
        {
            var key = _createKey.Invoke(item);
            if (string.IsNullOrWhiteSpace(key)) return;
            _index[key] = item;
        }
    }
}