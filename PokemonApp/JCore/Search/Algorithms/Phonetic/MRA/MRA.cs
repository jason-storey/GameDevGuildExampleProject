using System;
using System.Text;
namespace JCore.Search
{
    public static class MRA
    {
        public static int GetScore(string word1, string word2)
        {
            if (string.IsNullOrEmpty(word1) || string.IsNullOrEmpty(word2)) return 0;

            word1 = Preprocess(word1);
            word2 = Preprocess(word2);

            var minLength = Math.Min(word1.Length, word2.Length);
            var maxLength = Math.Max(word1.Length, word2.Length);

            var range = maxLength / 2 - 1;

            var matches = 0;
            var transpositions = 0;

            for (var i = 0; i < minLength; i++)
            {
                var startPos = Math.Max(0, i - range);
                var endPos = Math.Min(i + range + 1, maxLength);

                var found = false;

                for (var j = startPos; j < endPos; j++)
                    if (word1[i] == word2[j])
                    {
                        matches++;

                        if (i != j) transpositions++;

                        found = true;
                        break;
                    }

                if (!found) break;
            }

            if (matches == 0) return 0;

            transpositions /= 2;

            var matchRating = (matches / word1.Length + matches / word2.Length + (matches - transpositions) / matches) /
                              3;

            return matchRating;
        }

        public static int ToMRAScore(this string word,string word2) => GetScore(word, word2);

        static string Preprocess(string word)
        {
            word = word.ToUpper();
            word = RemoveVowels(word);
            word = SimplifyConsonants(word);
            return word.Length > 6 ? word.Substring(0, 6) : word;
        }

        static string RemoveVowels(string word)
        {
            var sb = new StringBuilder();
            sb.Append(word[0]);

            foreach (var c in word.Substring(1))
                if ("AEIOU".IndexOf(c) == -1)
                    sb.Append(c);

            return sb.ToString();
        }

        static string SimplifyConsonants(string word) => word
                .Replace("B", "P")
                .Replace("C", "K")
                .Replace("D", "T")
                .Replace("G", "K")
                .Replace("H", "")
                .Replace("J", "K")
                .Replace("Q", "K")
                .Replace("V", "F")
                .Replace("W", "")
                .Replace("X", "KS")
                .Replace("Y", "")
                .Replace("Z", "S");
    }
}