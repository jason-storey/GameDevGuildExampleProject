using System;
using System.Collections.Generic;
using System.Linq;

namespace JCore.Search
{
    public class SearchEngineV1<T> : ISearchEngine<T>
    {
        readonly IReadonlyRepository<T> _repo;
        readonly IAutoCompleteProvider<T> _autocomplete;
        readonly ISpellingCorrectionProvider _spelling;
        readonly PropertySelectors<T> _properties;

        public SearchEngineV1(
            IReadonlyRepository<T> repo,
            IAutoCompleteProvider<T> autocomplete,
            ISpellingCorrectionProvider spelling,PropertySelectors<T> properties)
        {
            _repo = repo;
            _autocomplete = autocomplete;
            _spelling = spelling;
            _properties = properties;
        }

        
        public IEnumerable<T> Search(string query) => _repo;
        public bool TrySpellcheck(string search, out string suggestion)
        {
            suggestion = _spelling.CorrectSpelling(search);
            return !suggestion.Equals(search) && !string.IsNullOrWhiteSpace(suggestion);
        }

        public IEnumerable<T> Search(Func<T, bool> predicate) => _repo.Where(predicate);
        public void AddRange(params T[] items)
        {
            _autocomplete.AddRange(items);
            foreach (var item in items) 
                _spelling.AddRange(_properties.Select(item).ToArray());
        }

        public IEnumerable<string> AutoComplete(string input) => _autocomplete.GetSuggestions(_spelling.CorrectSpelling(input));
    }
}