﻿namespace JCore.Search
{
    public class NYSSISAlgorithm : CachedStringAlgorithm
    {
        public NYSSISAlgorithm(IRepository<string> cache) : base(cache, NYSSIS.Generate) { }
    }
}