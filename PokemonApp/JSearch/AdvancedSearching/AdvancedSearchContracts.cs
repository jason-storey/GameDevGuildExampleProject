using System.Collections.Generic;

namespace JCore.AdvancedSearch
{
    public interface ISearchEngine<T>
    {
        IEnumerable<string> AutoComplete(string query);
        bool HasSpellcheckSuggestion(string search, out string suggestion);
        (List<T> Results, List<Facet> Facets) Search(string query, Dictionary<string, string> filters = null);
    }

    public interface ITokenizer
    {
        List<string> Tokenize(string input);
    }

    public interface ISynonymProvider
    {
        List<string> GetSynonyms(string word);
    }

    public interface ICache<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool TryGet(TKey key, out TValue value);
    }

    public class FacetOption
    {
        public string Value { get; set; }
        public int Count { get; set; }
    }

    public class Facet
    {
        public string Name { get; set; }
        public List<FacetOption> Options { get; set; } = new List<FacetOption>();
    }
}