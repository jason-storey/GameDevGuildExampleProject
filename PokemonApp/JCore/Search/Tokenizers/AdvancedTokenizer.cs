using System.Collections.Generic;
using System.Text;

namespace JCore
{
    public class AdvancedTokenizer : ITokenizer
    {
        private readonly List<char> _punctuationMarks = new List<char> { '.', ',', '!', '?', ';', ':', '-', '(', ')', '[', ']', '{', '}' };

        public IList<string> Tokenize(string text)
        {
            List<string> tokens = new List<string>();

            StringBuilder currentToken = new StringBuilder();
            bool inWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];

                if (char.IsLetter(currentChar))
                {
                    currentToken.Append(currentChar);
                    inWord = true;
                }
                else if (_punctuationMarks.Contains(currentChar))
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
                    if (!inWord) continue;
                    tokens.Add(currentToken.ToString());
                    currentToken.Clear();
                    inWord = false;
                }
            }

            if (inWord) tokens.Add(currentToken.ToString());

            return tokens;
        }
    }
}