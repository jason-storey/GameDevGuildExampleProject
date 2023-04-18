using System;
using System.Collections.Generic;
using System.Linq;

namespace JCore.Search
{
    public class SearchEngineV1<T> : ISearchEngine<T>
    {
        readonly IReadonlyRepository<T> _repo;
        readonly IAutoCompleteProvider<T> _autocomplete;

        public SearchEngineV1(IReadonlyRepository<T> repo,IAutoCompleteProvider<T> autocomplete)
        {
            _repo = repo;
            _autocomplete = autocomplete;
        }

        public IEnumerable<T> Search(string query) => _repo;
        public IEnumerable<T> Search(Func<T, bool> predicate) => _repo.Where(predicate);
        public IEnumerable<string> AutoComplete(string input) => _autocomplete.GetSuggestions(input);
    }
}