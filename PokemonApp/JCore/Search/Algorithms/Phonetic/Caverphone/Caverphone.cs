using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using JCore.Collections;

namespace JCore.Search
{
    public static class Caverphone
    {
        public static string Generate(string word)
        {
            if (string.IsNullOrEmpty(word)) return "";
            word = word.ToUpper();
            word = Regex.Replace(word, "[AEIOU]", "A");
            word = word
                .Replace("B", "P")
                .Replace("C", "K")
                .Replace("D", "T")
                .Replace("F", "P")
                .Replace("G", "K")
                .Replace("H", "A")
                .Replace("J", "Y")
                .Replace("L", "R")
                .Replace("M", "N")
                .Replace("Q", "K")
                .Replace("V", "P")
                .Replace("W", "A")
                .Replace("X", "K")
                .Replace("Z", "S");
            var sb = new StringBuilder();
            sb.Append(word[0]);
            for (int i = 1; i < word.Length; i++)
                if (word[i] != word[i - 1])
                    sb.Append(word[i]);
            word = sb.ToString();
            if (word.Length > 10) word = word.Substring(0, 10);
            else if (word.Length < 10) word = word.PadRight(10, 'A');
            return word;
        }
        
        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);
        public static string ToCaverphonee(this string s) => Generate(s);

        public static HashedListDictionary<string> CreateCache(IEnumerable<string> elements)
        {
            var cache = new HashedListDictionary<string>();
            foreach (var element in elements) 
                cache.AddElement(Generate(element), element);
            return cache;
        }
        
        public static bool MatchesAsCaverphone(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }
    }
}