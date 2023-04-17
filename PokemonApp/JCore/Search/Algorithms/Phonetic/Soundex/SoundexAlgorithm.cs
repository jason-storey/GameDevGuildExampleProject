using JCore.Search;

namespace JCore.Common
{
    public class SoundexAlgorithm : CachedStringAlgorithm
    {
        public SoundexAlgorithm() : this(new Index<string>()) { }
        public SoundexAlgorithm(IRepository<string> cache) : base(cache, Soundex.Generate) { }
    }
}