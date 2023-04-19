using System.Collections.Generic;
using System.Linq;
using JCore.AdvancedSearch;
using JCore.Collections;
namespace JSearch
{
    public class SimpleSynonymProvider : ISynonymProvider
    {
        readonly InvertedIndex<string> _synonyms;
        public SimpleSynonymProvider(InvertedIndex<string> synonyms) => _synonyms = synonyms;
        public List<string> GetSynonyms(string word) => _synonyms[word].ToList();
    }
}