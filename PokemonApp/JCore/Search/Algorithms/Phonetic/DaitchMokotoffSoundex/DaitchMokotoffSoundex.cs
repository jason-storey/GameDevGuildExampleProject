using System;
using System.Collections.Generic;
using System.Linq;
using JCore.Collections;

namespace JCore.Search
{
    public static class DaitchMokotoffSoundex
    {
        public static bool Matches(IEnumerable<string> first,IEnumerable<string> second)
        {
            if (first == null || second == null) return false;
            return first.Any(second.Contains);
        }

        public static List<string> Generate(string word)
        {
            if (string.IsNullOrEmpty(word)) return new List<string> { "" };
            word = word.ToUpper();
            List<string> codes = new List<string> { "" };
            int position = 0;

            while (position < word.Length)
            {
                var ruleMatch = FindRuleMatch(word, position);
                if (ruleMatch.Key != null)
                {
                    int ruleLength = ruleMatch.Key.Length;
                    var newCodes = new List<string>();

                    foreach (string code in ruleMatch.Value)
                    {
                        if (position == 0 && code == "0") continue;
                        foreach (string existingCode in codes) newCodes.Add(existingCode + code);
                    }

                    codes = newCodes;
                    position += ruleLength;
                }
                else position++;
            }
            for (int i = 0; i < codes.Count; i++)
                codes[i] = codes[i].Length > 6 ? codes[i].Substring(0, 6) : codes[i].PadRight(6, '0');
            return codes.Distinct().ToList();
        }

        static KeyValuePair<string, string[]> FindRuleMatch(string word, int position)
        {
            foreach (var rule in Rules.Where(rule => word.IndexOf(rule.Key, position, StringComparison.Ordinal) == position))
                return rule;
            return default;
        }

        static readonly Dictionary<string, string[]> Rules = new Dictionary<string, string[]>
        {
            { "AI", new[] { "01" } },
            { "AJ", new[] { "01" } },
            { "AY", new[] { "01" } },
            { "AU", new[] { "07" } },
            { "EI", new[] { "01" } },
            { "EJ", new[] { "01" } },
            { "EY", new[] { "01" } },
            { "EU", new[] { "01" } },
            { "OI", new[] { "01" } },
            { "OJ", new[] { "01" } },
            { "OY", new[] { "01" } },
            { "OU", new[] { "07" } },
            { "UI", new[] { "01" } },
            { "UJ", new[] { "01" } },
            { "UY", new[] { "01" } },
            { "IU", new[] { "01" } },
            { "A", new[] { "0" } },
            { "E", new[] { "0" } },
            { "I", new[] { "0" } },
            { "O", new[] { "0" } },
            { "U", new[] { "0" } },
            { "KS", new[] { "45" } },
            { "KH", new[] { "5" } },
            { "K", new[] { "5" } },
            { "Q", new[] { "5" } },
            { "C", new[] { "5" } },
            { "G", new[] { "5" } },
            { "Y", new[] { "1", "5" } },
            { "J", new[] { "1", "5" } },
            { "Z", new[] { "4" } },
            { "S", new[] { "4" } },
            { "TS", new[] { "44" } },
            { "TCH", new[] { "44" } },
            { "TSH", new[] { "44" } },
            { "CH", new[] { "4", "5" } },
            { "SH", new[] { "4", "5" } },
            { "TZ", new[] { "4", "6" } },
            { "TC", new[] { "4", "6" } },
            { "DS", new[] { "4" } },
            { "TS", new[] { "4" } },
            { "T", new[] { "3" } },
            { "D", new[] { "3" } },
            { "WR", new[] { "94" } },
            { "V", new[] { "7" } },
            { "W", new[] { "7" } },
            { "WH", new[] { "7" } },
            { "FP", new[] { "7" } },
            { "B", new[] { "7" } },
            { "P", new[] { "7" } },
            { "F", new[] { "7" } },
            { "M", new[] { "6" } },
            { "N", new[] { "6" } },
            { "H", new[] { "5" } },
            { "L", new[] { "8" } },
            { "R", new[] { "9" } },
            { "X", new[] { "54" } }
        };
        
        public static HashedListDictionary<string> CreateCache(IEnumerable<string> elements)
        {
            var cache = new HashedListDictionary<string>();
            foreach (var element in elements)
            {
                var all = Generate(element);
                foreach (var item in all) 
                    cache.AddElement(item, element);
            }
            return cache;
        }
        
        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);
        public static List<string> ToDaitchMokotoffSoundexes(this string s) => Generate(s);

        public static bool MatchesDaitchMokotoffSoundex(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }
    }
}