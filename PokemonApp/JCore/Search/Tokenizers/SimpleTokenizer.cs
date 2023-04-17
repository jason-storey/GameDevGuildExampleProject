using System.Collections.Generic;
using System.Linq;

namespace JCore
{
    public class SimpleTokenizer : ITokenizer
    {
        public IList<string> Tokenize(string text) => text.Split(' ').ToList();
    }
}