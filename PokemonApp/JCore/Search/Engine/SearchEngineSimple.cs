using System.Collections.Generic;
using System.Linq;
using JCore.Collections;
using JCore.Extensions;
namespace JCore.Search
{
    public class SearchEngineSimple
    {
        AutocompleteTrie _autoComplete;
        BKTree _spellcheck;
        HashedListDictionary<string> _phonetics;
        public SearchEngineSimple(List<string> documents)
        {
            _autoComplete = new AutocompleteTrie();
            _spellcheck = new BKTree();
            foreach (string document in documents)
            {
                _autoComplete.Insert(document);
                _spellcheck.Insert(document);
                _phonetics = Soundex.CreateCache(documents);
            }
        }

        public IList<string> Search(string query, int maxEditDistance = 2)
        {
            var suggestions = _autoComplete.Search(query);
            var correctedQueries = _spellcheck.Query(query, maxEditDistance);
            foreach (var correctedSuggestions in correctedQueries.Select(correctedQuery => _autoComplete.Search(correctedQuery)))
                suggestions.AddRange(correctedSuggestions);
            suggestions.AddRange( _phonetics.GetValuesById(query.ToSoundex()));
            suggestions.RemoveDuplicates();
           return RankDocuments(query, suggestions);
        }

        List<string> RankDocuments(string query, List<string> documents) => 
            new BM25(documents,new AdvancedTokenizer()).RankDocuments(query);
    }
}