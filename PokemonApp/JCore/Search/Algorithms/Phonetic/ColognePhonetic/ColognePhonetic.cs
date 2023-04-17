using System.Text;

namespace JCore.Search
{
    public static class ColognePhonetic
    {
        public static string Generate(string word)
        {
            if (string.IsNullOrEmpty(word)) return "";
            word = word.ToUpper();
            var sb = new StringBuilder();
            for (var i = 0; i < word.Length; i++)
            {
                var currentChar = word[i];
                var current = currentChar.ToString();
                var code = "";

                if (char.IsLetter(currentChar))
                {
                    if ("AEIOUJY".Contains(current))
                        code = "0";
                    else if ("B".Contains(current))
                        code = "1";
                    else if ("P".Contains(current))
                        code = "1";
                    else if ("L".Contains(current))
                        code = "5";
                    else if ("M".Contains(current))
                        code = "6";
                    else if ("N".Contains(current))
                        code = "6";
                    else if ("D".Contains(current) || "T".Contains(current))
                    {
                        if (i + 1 < word.Length)
                        {
                            var next = word[i + 1].ToString();
                            code = "CSZ".Contains(next) ? "8" : "2";
                        }
                        else
                            code = "2";
                    }
                    else if ("F".Contains(current) || "V".Contains(current) || "W".Contains(current))
                        code = "3";
                    else if ("G".Contains(current) || "K".Contains(current) || "Q".Contains(current))
                        code = "4";
                    else if ("C".Contains(current))
                    {
                        if (i + 1 < word.Length)
                        {
                            var next = word[i + 1].ToString();
                            code = "AHKLOQRUX".Contains(next) ? "4" : "8";
                        }
                        else
                            code = "8";
                    }
                    else if ("X".Contains(current))
                    {
                        if (i == 0 && (word.Length == 1 || "CKQ".Contains(word[i + 1].ToString())))
                            code = "48";
                        else code = "8";
                    }
                    else if ("R".Contains(current))
                        code = "7";
                    else if ("SZ".Contains(current)) code = "8";
                }

                if (!string.IsNullOrEmpty(code) && (sb.Length == 0 || sb[sb.Length - 1].ToString() != code))
                    sb.Append(code);
            }
            return sb.ToString();
        }
        
        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);
        public static string ToColognePhonetic(this string s) => Generate(s);

        public static bool MatchesAsColognePhonetic(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }
        
    }
}