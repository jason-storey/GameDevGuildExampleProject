using System.Collections.Generic;

namespace JCore
{
    public interface ITokenizer
    {
        IEnumerable<string> Tokenize(string text);
    }
}