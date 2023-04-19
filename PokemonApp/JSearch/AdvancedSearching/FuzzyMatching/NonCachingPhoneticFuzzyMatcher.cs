using JCore.Search;

namespace JCore.AdvancedSearch
{
    public class NonCachingPhoneticFuzzyMatcher : IFuzzyMatcher
    {
        readonly PhoneticMatchAlgorithms _algorithm;

        public NonCachingPhoneticFuzzyMatcher(PhoneticMatchAlgorithms algorithm) => _algorithm = algorithm;
        public bool FuzzyMatch(string a, string b) => a.Matches(b, _algorithm);
    }
}