using System.Collections.Generic;
using System.Text;

namespace JCore
{
    public class BooleanTokenizer : ITokenizer
    {
        readonly List<char> _booleanOperators = new List<char> { '&', '|', '(', ')' };
        readonly List<char> _wildcardOperators = new List<char> { '*' };

        public IEnumerable<string> Tokenize(string text)
        {
            IList<string> tokens = new List<string>();

            StringBuilder currentToken = new StringBuilder();
            bool inWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];

                if (char.IsLetterOrDigit(currentChar))
                {
                    currentToken.Append(currentChar);
                    inWord = true;
                }
                else if (_booleanOperators.Contains(currentChar) || _wildcardOperators.Contains(currentChar))
                {
                    if (inWord)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                        inWord = false;
                    }

                    tokens.Add(currentChar.ToString());
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    if (inWord)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                        inWord = false;
                    }
                }
            }

            if (inWord) tokens.Add(currentToken.ToString());

            return tokens;
        }
    }
}