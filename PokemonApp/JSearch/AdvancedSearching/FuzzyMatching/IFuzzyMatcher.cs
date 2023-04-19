using System.Collections.Generic;
using System.Linq;
using JCore.Collections;

namespace JCore.AdvancedSearch
{
    public interface IFuzzyMatcher
    {
        bool FuzzyMatch(string a, string b);
    }
    
    public abstract class SingleResultCachedFuzzyMatcher<T> : IFuzzyMatcher
    {
        readonly ICache<string, T> _cache;

        public SingleResultCachedFuzzyMatcher(ICache<string,T> cache) => 
            _cache = cache;

        public bool FuzzyMatch(string a, string b) => IsMatch(a,GetOrAdd(a),b,GetOrAdd(b));

        protected virtual bool IsMatch(string a, T aToken, string b, T bToken) => aToken.Equals(bToken);
        
        T GetOrAdd(string token)
        {
            if (_cache.TryGet(token, out var soundexA)) return soundexA;
            soundexA = Generate(token);
            _cache.Add(token, soundexA);
            return soundexA;
        }

        protected abstract T Generate(string token);
    }
    
    
    public abstract class ListResultFuzzyMatcher : IFuzzyMatcher
    {
        readonly InvertedIndex<string> _cache;

        public ListResultFuzzyMatcher(InvertedIndex<string> cache) => _cache = cache;

        protected abstract IEnumerable<string> Generates(string token);

        IEnumerable<string> GetOrAdd(string token)
        {
            if (_cache.TryGet(token, out var results)) return results;
            var newResults = Generates(token).ToArray();
            _cache.Add(token, newResults);
            return newResults;  
        }

        public bool FuzzyMatch(string a, string b) => GetOrAdd(a).Intersect(GetOrAdd(b)).Any();
    }
}