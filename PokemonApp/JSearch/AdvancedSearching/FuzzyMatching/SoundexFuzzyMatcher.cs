using System.Collections.Generic;
using JCore.Collections;
using JCore.Search;

namespace JCore.AdvancedSearch
{
    public class SoundexFuzzyMatcher : SingleResultCachedFuzzyMatcher<string>
    {
        public SoundexFuzzyMatcher(ICache<string, string> cache) : base(cache) { }
        protected override string Generate(string token) => token.ToSoundex();
    }
    
    public class MetaphoneFuzzyMatcher : SingleResultCachedFuzzyMatcher<string>
    {
        protected override string Generate(string token) => token.ToMetaphone();

        public MetaphoneFuzzyMatcher(ICache<string, string> cache) : base(cache) { }
    }

    public class CaverphoneFuzzyMatcher : SingleResultCachedFuzzyMatcher<string>
    {
        public CaverphoneFuzzyMatcher(ICache<string, string> cache) : base(cache) { }
        protected override string Generate(string token) => token.ToCaverphonee();
    }

    public class ColognePhoneticFuzzyMatcher : SingleResultCachedFuzzyMatcher<string>
    {
        public ColognePhoneticFuzzyMatcher(ICache<string, string> cache) : base(cache) { }
        protected override string Generate(string token) => token.ToColognePhonetic();
    }

    public class DaitchMokotoffSoundexFuzzyMatcher : ListResultFuzzyMatcher
    {
        public DaitchMokotoffSoundexFuzzyMatcher(InvertedIndex<string> cache) : base(cache) { }
        protected override IEnumerable<string> Generates(string token) => token.ToDaitchMokotoffSoundexes();
    }

}