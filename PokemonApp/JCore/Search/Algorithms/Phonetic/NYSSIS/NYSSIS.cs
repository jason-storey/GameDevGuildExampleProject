using System.Text;
namespace JCore.Search
{
    public static class NYSSIS
    {
        public static string Generate(string word)
        {
            if (string.IsNullOrEmpty(word)) return "";
            word = word.ToUpper();
            if (word.Length == 1) return word;
            var result = new StringBuilder();
            if (word.StartsWith("MAC")) word = "MCC" + word.Substring(3);
            else if (word.StartsWith("KN")) word = "NN" + word.Substring(2);
            else if (word.StartsWith("K")) word = "C" + word.Substring(1);
            else if (word.StartsWith("PH")) word = "FF" + word.Substring(2);
            else if (word.StartsWith("PF")) word = "FF" + word.Substring(2);
            else if (word.StartsWith("SCH")) word = "SSS" + word.Substring(3);
            result.Append(word[0]);
            for (var i = 1; i < word.Length; i++)
            {
                var current = word[i];

                switch (current)
                {
                    case 'A':
                    case 'E':
                    case 'I':
                    case 'O':
                    case 'U':
                    case 'Y':
                        current = 'A';
                        break;
                    case 'H':
                        current = word[i - 1];
                        break;
                    case 'W':
                        current = 'A';
                        break;
                    case 'D':
                    case 'T':
                        current = 'D';
                        break;
                    case 'M':
                    case 'N':
                        current = 'N';
                        break;
                    case 'K':
                        current = 'C';
                        break;
                    case 'R':
                        current = 'R';
                        break;
                    case 'S':
                        current = 'S';
                        break;
                    case 'L':
                        current = 'L';
                        break;
                    case 'J':
                    case 'Z':
                        current = 'Z';
                        break;
                    case 'F':
                    case 'V':
                        current = 'F';
                        break;
                    case 'B':
                    case 'P':
                        current = 'P';
                        break;
                    default:
                    {
                        if (current == 'G' || current == 'K' || current == 'Q')
                        {
                            current = 'K';
                        }
                        else if (current == 'C')
                        {
                            if (i < word.Length - 1)
                            {
                                var next = word[i + 1];
                                if (next == 'H') current = 'S';
                                else current = 'K';
                            }
                            else
                            {
                                current = 'K';
                            }
                        }
                        else if (current == 'X')
                        {
                            current = 'S';
                        }

                        break;
                    }
                }

                if (result[result.Length - 1] != current) result.Append(current);
            }

            if (result[result.Length - 1] == 'S') result.Length--;
            result.Append("AY");
            return result.ToString();
        }


        public static bool Match(this string word1, string word2) => Generate(word1) == Generate(word2);

        public static string ToNYSSIS(this string s) => Generate(s);

        public static bool MatchesAsNYSSIS(this string s, string other)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(other)) return false;
            return Generate(s) == Generate(other);
        }

    }
}