using System;
using System.Collections.Generic;
using System.Linq;

namespace JCore.Search
{
    public class Autocomplete<T> : IAutoCompleteProvider<T>
    {
        readonly PropertySelectors<T> _properties;

        public Autocomplete(PropertySelectors<T> properties)
        {
            _properties = properties;
            _trie = new AutocompleteTrie();
            _data = new HashSet<T>();
        }

        readonly HashSet<T> _data;
        public void AddSelector(Func<T, string> selector)
        {
            _properties.Add(selector);
            UpdateDatasetForNewSelector(selector);
        }

        void UpdateDatasetForNewSelector(Func<T,string> selector)
        {
            foreach (var entry in _data) 
                _trie.Insert(selector.Invoke(entry));
        }
        
        public void Add(T item)
        {
            _data.Add(item);
            ApplySelectors(item);
        }

        void ApplySelectors(T data) => _trie.InsertAll(_properties.Select(data).ToArray());

        public IEnumerable<string> GetSuggestions(string input) => _trie.Search(input);
        public void AddRange(params T[] items)
        {
            foreach (var item in items) 
                Add(item);
        }

        readonly AutocompleteTrie _trie;
    }
}