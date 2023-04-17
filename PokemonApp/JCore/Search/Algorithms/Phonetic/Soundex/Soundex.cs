using System;
using System.Text;

namespace JCore
{
    public static class Soundex
    {
        public static string Generate(string word)
        {
            if (string.IsNullOrEmpty(word))
                return string.Empty;

            var soundexCode = new StringBuilder();
            word = word.ToUpper();

            soundexCode.Append(word[0]);

            for (int i = 1; i < word.Length; i++)
            {
                char soundexDigit = GetSoundexDigit(word[i]);
                if (soundexDigit != '0' && soundexDigit != soundexCode[soundexCode.Length - 1])
                    soundexCode.Append(soundexDigit);
            }

            soundexCode.Length = Math.Min(4, soundexCode.Length);

            while (soundexCode.Length < 4) soundexCode.Append('0');

            return soundexCode.ToString();
        }

        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);

        public static string ToSoundex(this string s) => Generate(s);

        public static bool MatchesAsSoundexes(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }

        static char GetSoundexDigit(char c)
        {
            switch (c)
            {
                case 'B':
                case 'F':
                case 'P':
                case 'V':
                    return '1';
                case 'C':
                case 'G':
                case 'J':
                case 'K':
                case 'Q':
                case 'S':
                case 'X':
                case 'Z':
                    return '2';
                case 'D':
                case 'T':
                    return '3';
                case 'L':
                    return '4';
                case 'M':
                case 'N':
                    return '5';
                case 'R':
                    return '6';
                default:
                    return '0';
            }
        }
    }
}