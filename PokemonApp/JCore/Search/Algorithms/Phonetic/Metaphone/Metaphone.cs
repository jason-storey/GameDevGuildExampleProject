using System;
using System.Text;

namespace JCore.Common
{
    public static class Metaphone
    {
        public static string Generate(string word)
        {
            if (string.IsNullOrEmpty(word)) return "";

            word = word.ToUpper();

            if (word.Length == 1) return word;

            var result = new StringBuilder(word.Length);

            for (var i = 0; i < word.Length; i++)
            {
                var current = word[i];

                if (i == 0 || !IsVowel(current.ToString()))
                    switch (current)
                    {
                        case 'A':
                        case 'E':
                        case 'I':
                        case 'O':
                        case 'U':
                            if (i == 0) result.Append(current);
                            break;
                        case 'B':
                            result.Append("B");
                            break;
                        case 'C':
                            if (i < word.Length - 1)
                            {
                                var next = word[i + 1];
                                if (next != 'H')
                                {
                                    result.Append("K");
                                }
                                else
                                {
                                    result.Append("X");
                                    i++;
                                }
                            }

                            break;
                        case 'D':
                            result.Append("T");
                            break;
                        case 'F':
                            result.Append("F");
                            break;
                        case 'G':
                            result.Append("K");
                            break;
                        case 'H':
                            if (i > 0 && !IsVowel(word[i - 1].ToString())) result.Append("H");
                            break;
                        case 'J':
                            result.Append("J");
                            break;
                        case 'K':
                            result.Append("K");
                            break;
                        case 'L':
                            result.Append("L");
                            break;
                        case 'M':
                            result.Append("M");
                            break;
                        case 'N':
                            result.Append("N");
                            break;
                        case 'P':
                            result.Append("P");
                            break;
                        case 'Q':
                            result.Append("K");
                            break;
                        case 'R':
                            result.Append("R");
                            break;
                        case 'S':
                            if (i < word.Length - 1)
                            {
                                var next = word[i + 1];
                                if (next != 'H')
                                {
                                    result.Append("S");
                                }
                                else
                                {
                                    result.Append("X");
                                    i++;
                                }
                            }

                            break;
                        case 'T':
                            result.Append("T");
                            break;
                        case 'V':
                            result.Append("F");
                            break;
                        case 'W':
                            result.Append("W");
                            break;
                        case 'X':
                            result.Append("KS");
                            break;
                        case 'Y':
                            result.Append("Y");
                            break;
                        case 'Z':
                            result.Append("S");
                            break;
                    }
            }

            return result.ToString();
        }

        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);
        public static string ToMetaphone(this string s) => Generate(s);

        public static bool MatchesAsMetaphone(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }

        static bool IsVowel(string letter) => Array.IndexOf(vowels, letter) >= 0;
        static readonly string[] vowels = { "A", "E", "I", "O", "U" };
    }
}