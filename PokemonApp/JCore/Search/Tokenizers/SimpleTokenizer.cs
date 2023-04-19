using System.Collections.Generic;
using System.Linq;

namespace JCore
{
    public class SimpleTokenizer : ITokenizer
    {
        public IEnumerable<string> Tokenize(string text) => text.Split(' ').ToList();
    }
}