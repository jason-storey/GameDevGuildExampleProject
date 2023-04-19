using System;

namespace JSearch.Search
{
    public static class JaroWinkler
    {
        const double DEFAULT_THRESHOLD = 0.85;
        public static bool FuzzyMatch(string token, string propertyString) => 
            CalculateScore(token, propertyString) >= DEFAULT_THRESHOLD;

        public static double ToJaroWinklerScore(this string a, string b) => CalculateScore(a, b);
        
        public static double CalculateScore(string s1, string s2, double p = 0.1)
        {
            int m = 0, t = 0;
            if (s1.Length == 0 || s2.Length == 0) return 0;

            int[] matchRange = { Math.Max(0, Math.Max(s1.Length, s2.Length) / 2 - 1), 0 };
            bool[] s1Matches = new bool[s1.Length], s2Matches = new bool[s2.Length];

            for (int i = 0; i < s1.Length; i++)
            {
                for (int j = Math.Max(0, i - matchRange[0]); j < Math.Min(s2.Length, i + matchRange[0] + 1); j++)
                {
                    if (s1Matches[i] || s2Matches[j] || s1[i] != s2[j]) continue;
                    m++;
                    s1Matches[i] = s2Matches[j] = true;
                    break;
                }
            }
            if (m == 0) return 0;

            int k = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (!s1Matches[i]) continue;
                while (!s2Matches[k]) k++;
                if (s1[i] != s2[k]) t++;
                k++;
            }
            t /= 2;
            double dj = ((double)m / s1.Length + (double)m / s2.Length + (double)(m - t) / m) / 3;

            int l = 0;
            for (; l < Math.Min(4, Math.Min(s1.Length, s2.Length)); l++) if (s1[l] != s2[l]) break;

            return dj + l * p * (1 - dj);
        }
    }
}