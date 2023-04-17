using System;

namespace JCore.Search
{
    public static class Levenshtein
    {
        public static int Calculate(string s1, string s2) {
            int m = s1.Length, n = s2.Length;
            var d = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++) d[i, 0] = i;
            for (int j = 0; j <= n; j++) d[0, j] = j;
            for (int i = 1; i <= m; i++) {
                for (int j = 1; j <= n; j++) {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[m, n];
        }


        public static bool HasLevenshteinLowEnough(this string s, string other, int target = 3) =>
            Calculate(s, other) < target;
        public static int ToLevenshtein(this string s, string other) => Calculate(s, other);
    }
}