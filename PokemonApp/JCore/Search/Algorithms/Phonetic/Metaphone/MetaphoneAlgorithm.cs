using JCore.Collections;

namespace JCore.Search
{
    public class MetaphoneAlgorithm : CachedStringAlgorithm
    {
        public MetaphoneAlgorithm() : this(new Index<string>()) { }
        public MetaphoneAlgorithm(IRepository<string> cache) : base(cache, Metaphone.Generate) { }
    }
}