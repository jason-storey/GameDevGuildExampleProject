using System.Collections.Generic;
using System.Linq;

namespace JCore
{
    public class StopwordRemovalTokenizer : ITokenizer
    {
        readonly List<string> _stopwords = new List<string> { "the", "and", "or", "a", "an" };

        public IEnumerable<string> Tokenize(string text) => 
            text.Split(' ').Where(word => !_stopwords.Contains(word.ToLower())).ToList();
    }
}