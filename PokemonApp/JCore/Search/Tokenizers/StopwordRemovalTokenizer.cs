using System.Collections.Generic;

namespace JCore
{
    public class StopwordRemovalTokenizer : ITokenizer
    {
        readonly List<string> _stopwords = new List<string> { "the", "and", "or", "a", "an" };

        public IList<string> Tokenize(string text)
        {
            IList<string> tokens = new List<string>();

            foreach (string word in text.Split(' '))
            {
                if (!_stopwords.Contains(word.ToLower())) tokens.Add(word);
            }
            return tokens;
        }
    }
}