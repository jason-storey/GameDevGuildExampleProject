using System.Collections.Generic;

namespace JCore
{
    public interface ITokenizer
    {
        IList<string> Tokenize(string text);
    }
}