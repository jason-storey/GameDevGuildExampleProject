using System;
using System.Collections.Generic;

namespace JCore.Search
{
    public class Autocomplete<T> : IAutoCompleteProvider<T>
    {
        readonly List<Func<T, string>> _selectors;
        public Autocomplete()
        {
            _selectors = new List<Func<T, string>>();
            _trie = new AutocompleteTrie();
            _data = new HashSet<T>();
        }

        readonly HashSet<T> _data;
        public void AddSelector(Func<T, string> selector)
        {
            _selectors.Add(selector);
            UpdateDatasetForNewSelector(selector);
        }

        void UpdateDatasetForNewSelector(Func<T,string> selector)
        {
            foreach (var entry in _data) 
                _trie.Insert(selector.Invoke(entry));
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items) 
                Add(item);
        }

        public void Add(T item)
        {
            _data.Add(item);
            ApplySelectors(item);
        }

        void ApplySelectors(T data)
        {
            foreach (var selector in _selectors) 
                _trie.Insert(selector?.Invoke(data));
        }
        
        public IEnumerable<string> GetSuggestions(string input) => _trie.Search(input);

        readonly AutocompleteTrie _trie;
    }
}