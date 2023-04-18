using System;
namespace JCore.Search
{
    public abstract class CachedStringAlgorithm : ISearchAlgorithm
    {
        readonly IRepository<string> _cache;
        readonly Func<string, string> _algorithm;
        
        protected CachedStringAlgorithm(IRepository<string> cache,Func<string,string> algorithm)
        {
            _cache = cache;
            _algorithm = algorithm;
        }

        public bool IsMatch(string searchTerm, string content) => 
            Get(searchTerm).Equals(Get(content));

        public string Get(string term)
        {
            var item = _cache.GetById(term);
            if (!string.IsNullOrWhiteSpace(item)) return item;
            item = _algorithm.Invoke(term);
            _cache.Add(item);
            return item;
        }
    }
}