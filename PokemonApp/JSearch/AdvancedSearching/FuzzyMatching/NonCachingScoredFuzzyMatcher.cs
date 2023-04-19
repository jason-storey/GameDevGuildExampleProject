using System;
using JCore.Search;

namespace JCore.AdvancedSearch
{
    public class NonCachingScoredFuzzyMatcher : IFuzzyMatcher
    {
        readonly ScoreMatchingAlgorithms _algorithm;
        readonly Func<int, bool> _matches;

        public NonCachingScoredFuzzyMatcher(ScoreMatchingAlgorithms algorithm, Func<int,bool> matches)
        {
            _algorithm = algorithm;
            _matches = matches;
        }
        public bool FuzzyMatch(string a, string b) => a.Matches(b,_matches, _algorithm);
    }
}