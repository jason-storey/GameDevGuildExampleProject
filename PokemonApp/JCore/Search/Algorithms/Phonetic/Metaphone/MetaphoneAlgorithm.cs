using JCore.Search;

namespace JCore.Common
{
    public class MetaphoneAlgorithm : CachedStringAlgorithm
    {
        public MetaphoneAlgorithm() : this(new Index<string>()) { }
        public MetaphoneAlgorithm(IRepository<string> cache) : base(cache, Metaphone.Generate) { }
    }
}