﻿using System;
using JCore.Search;

namespace JCore.Common
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

        string Get(string term)
        {
            var item = _cache.GetById(term);
            if (!string.IsNullOrWhiteSpace(item)) return item;
            item = _algorithm.Invoke(term);
            _cache.Create(term,item);
            return item;
        }
    }
}