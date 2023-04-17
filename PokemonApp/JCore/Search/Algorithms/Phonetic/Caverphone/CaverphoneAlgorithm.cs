using JCore.Common;

namespace JCore.Search.Algorithms
{
    public class CaverphoneAlgorithm : CachedStringAlgorithm
    {
        public CaverphoneAlgorithm(IRepository<string> cache) : base(cache, Caverphone.Generate) { }
    }
}