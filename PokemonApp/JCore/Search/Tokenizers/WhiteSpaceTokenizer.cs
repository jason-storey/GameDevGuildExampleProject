using System;
using System.Collections.Generic;

namespace JCore
{
    public class WhiteSpaceTokenizer : ITokenizer
    {
        public IEnumerable<string> Tokenize(string input) => 
            input.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
    }
}