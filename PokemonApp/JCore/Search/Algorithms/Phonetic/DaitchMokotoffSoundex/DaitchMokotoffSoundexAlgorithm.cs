using JCore.Collections;

namespace JCore.Search
{
    public class DaitchMokotoffSoundexAlgorithm : CachedStringListAlgorithm
    {
        public DaitchMokotoffSoundexAlgorithm() : this(new HashedListDictionary<string>()) { }
        public DaitchMokotoffSoundexAlgorithm(IListRepository<string> cache) : base(cache, DaitchMokotoffSoundex.Generate) { }
    }
}