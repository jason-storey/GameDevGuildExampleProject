using JCore.Collections;

namespace JCore.Search
{
    public class SoundexAlgorithm : CachedStringAlgorithm
    {
        public SoundexAlgorithm() : this(new Index<string>()) { }
        public SoundexAlgorithm(IRepository<string> cache) : base(cache, Soundex.Generate) { }
    }
}