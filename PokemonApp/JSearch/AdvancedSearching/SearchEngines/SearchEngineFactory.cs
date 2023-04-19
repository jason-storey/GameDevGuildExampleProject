using System.Collections.Generic;
using JCore;
using JCore.AdvancedSearch;
using JCore.Collections;
using JCore.Search;

namespace JSearch
{
    public class SearchEngineFactory<T>
    {
        readonly InvertedIndex<string> _synonyms;

        public SearchEngineFactory() : this(new InvertedIndex<string>()) { }
        public SearchEngineFactory(InvertedIndex<string> synonyms) => _synonyms = synonyms;
        public ISearchEngine<T> Create(IReadonlyRepository<T> items) => 
            new AdvancedSearchEngine<T>(items,
                new SimpleTokenizer(),
                new SimpleSynonymProvider(_synonyms),
                new SimpleCache<string, List<(double, T)>>(),
                new JaroWinkerFuzzyMatcher(),
                new BKLevensteinSpellingCorrector(),
                new AutocompleteFromTrie<T>());
    }
}