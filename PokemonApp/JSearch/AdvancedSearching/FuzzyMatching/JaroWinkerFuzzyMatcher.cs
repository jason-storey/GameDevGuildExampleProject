using JSearch.Search;
namespace JCore.AdvancedSearch
{
    public class JaroWinkerFuzzyMatcher : IFuzzyMatcher
    {
        public bool FuzzyMatch(string a, string b) => JaroWinkler.FuzzyMatch(a,b);
    }
}