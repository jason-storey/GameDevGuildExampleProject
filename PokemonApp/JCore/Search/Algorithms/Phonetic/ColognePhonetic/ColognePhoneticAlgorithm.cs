using JCore.Common;

namespace JCore.Search
{
    public class ColognePhoneticAlgorithm : CachedStringAlgorithm
    {
        public ColognePhoneticAlgorithm() : this(new Index<string>()) { }
        public ColognePhoneticAlgorithm(IRepository<string> cache) : base(cache, ColognePhonetic.Generate) { }
    }
}