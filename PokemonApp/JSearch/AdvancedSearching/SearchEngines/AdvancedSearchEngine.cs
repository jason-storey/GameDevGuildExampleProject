using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JCore.Search;

namespace JCore.AdvancedSearch
{
    public class AdvancedSearchEngine<T> : ISearchEngine<T>
    {
        readonly IReadonlyRepository<T> _repository;
        readonly ITokenizer _tokenizer;
        readonly ISynonymProvider _synonyms;
        readonly IFuzzyMatcher _fuzzySearch;
        readonly ISpellingCorrectionProvider _spellcheck;
        readonly IAutoCompleteProvider<T> _autoComplete;
        readonly ICache<string, List<(double, T)>> _cache;

        public AdvancedSearchEngine(IReadonlyRepository<T> repository, ITokenizer tokenizer, ISynonymProvider synonyms,
            ICache<string, List<(double, T)>> cache,IFuzzyMatcher fuzzySearch,ISpellingCorrectionProvider spellcheck,IAutoCompleteProvider<T> autoComplete)
        {
            _repository = repository;
            _tokenizer = tokenizer;
            _synonyms = synonyms;
            _cache = cache;
            _fuzzySearch = fuzzySearch;
            _spellcheck = spellcheck;
            _autoComplete = autoComplete;
        }

        public IEnumerable<string> AutoComplete(string query) => _autoComplete.GetSuggestions(query);

        public bool HasSpellcheckSuggestion(string search, out string suggestion)
        {
            suggestion = _spellcheck.CorrectSpelling(search);
            return !string.IsNullOrWhiteSpace(suggestion) && !search.Equals(suggestion);
        }

        public (List<T> Results, List<Facet> Facets) Search(string query, Dictionary<string, string> filters = null) =>
            _cache.TryGet(query, out List<(double, T)> cachedResults)
                ? ReturnResultsFromCache(cachedResults)
                : GenerateAndCacheSearchResults(query, filters);
        
        static void UpdateFacets(T item, Dictionary<string, Dictionary<string, int>> facets)
        {
            Type itemType = typeof(T);
            foreach (PropertyInfo property in itemType.GetProperties())
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(item);
                if (!facets.ContainsKey(propertyName)) facets[propertyName] = new Dictionary<string, int>();
                string facetValue = propertyValue.ToString();
                if (!facets[propertyName].ContainsKey(facetValue)) facets[propertyName][facetValue] = 0;
                facets[propertyName][facetValue]++;
            }
        }
        
        (List<T> Results, List<Facet> Facets) GenerateAndCacheSearchResults(string query,
            Dictionary<string, string> filters)
        {
            var queryTokens = _tokenizer.Tokenize(query);
            var results = new List<(double, T)>();
            var facets = new Dictionary<string, Dictionary<string, int>>();

            foreach (var item in _repository.GetAll())
            {
                if (!ApplyFilters(item, filters) || !IsMatch(item, queryTokens)) continue;
                double score = CalculateScore(item, queryTokens);
                results.Add((score, item));
                UpdateFacets(item, facets);
            }

            SortResultsByScore(results);
            var resultList = results.Select(result => result.Item2).ToList();
            var facetList = CreateFacetObjects(facets);
            _cache.Add(query, results);
            return (resultList, facetList);
        }
        
        static List<Facet> CreateFacetObjects(Dictionary<string, Dictionary<string, int>> facets) =>
            facets.Select(facet => new Facet
            {
                Name = facet.Key,
                Options = facet.Value.Select(option => new FacetOption { Value = option.Key, Count = option.Value })
                    .ToList()
            }).ToList();

        static void SortResultsByScore(List<(double, T)> results) => results.Sort((a, b) => b.Item1.CompareTo(a.Item1));

        static (List<T> Results, List<Facet> Facets) ReturnResultsFromCache(List<(double, T)> cachedResults)
        {
            var results = cachedResults.Select(result => result.Item2).ToList();
            var facetList = HandleFasceting(results);
            return (results, facetList);
        }

        static List<Facet> HandleFasceting(List<T> results)
        {
            var facets = new Dictionary<string, Dictionary<string, int>>();
            foreach (var item in results)
                UpdateFacets(item, facets);
            // Convert the facet dictionary into a list of Facet objects
            var facetList = CreateFacetObjects(facets);
            return facetList;
        }

        bool ApplyFilters(T item, Dictionary<string, string> filters)
        {
            if (filters == null || filters.Count == 0)
                return true;

            var itemType = typeof(T);
            return !filters.Select(filter => new { filter, property = itemType.GetProperty(filter.Key) })
                .Where(t => t.property != null)
                .Select(t => new { t, propertyValue = t.property.GetValue(item) })
                .Where(t => !t.propertyValue.ToString().Equals(t.t.filter.Value))
                .Select(t => t.t.filter).Any();
        }

        bool IsMatch(T item, List<string> queryTokens)
        {
            Type itemType = typeof(T);
            return queryTokens.Select(token => itemType.GetProperties()
                    .Select(property => property.GetValue(item))
                    .Any(propertyValue => propertyValue != null && propertyValue.ToString()
                        .IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0))
                .All(tokenMatch => tokenMatch);
        }

        double CalculateScore(T item, List<string> queryTokens)
        {
            double score = 0;
            Type itemType = typeof(T);

            foreach (var token in queryTokens)
            foreach (var property in itemType.GetProperties())
            {
                object propertyValue = property.GetValue(item);
                if (propertyValue == null) continue;
                string propertyString = propertyValue.ToString();
                // Add score for exact token match
                if (propertyString.Equals(token, StringComparison.OrdinalIgnoreCase)) score += 1;
                // Add score for fuzzy match
                if (_fuzzySearch.FuzzyMatch(token, propertyString)) score += 0.5;
                // Add score for synonym match
                if (_synonyms == null) continue;
                var synonyms = _synonyms.GetSynonyms(token);
                if (synonyms.Contains(propertyString, StringComparer.OrdinalIgnoreCase)) score += 0.25;
            }
            return score;
        }
    }
}