using System;
using JSearch.Search;

namespace JCore.Search
{
    public static class AlgorithmExtensions
    {
        public static bool Matches(this string a, string b, PhoneticMatchAlgorithms algorithm = PhoneticMatchAlgorithms.Soundex)
        {
            switch (algorithm)
            {
                case PhoneticMatchAlgorithms.Soundex:
                    return a.MatchesAsSoundexes(b);
                case PhoneticMatchAlgorithms.Caverphone:
                    return a.MatchesAsCaverphone(b);
                case PhoneticMatchAlgorithms.ColognePhonetic:
                    return a.MatchesAsColognePhonetic(b);
                case PhoneticMatchAlgorithms.DaitchMokotoff:
                    return a.MatchesDaitchMokotoffSoundex(b);
                case PhoneticMatchAlgorithms.Metaphone:
                    return a.MatchesAsMetaphone(b);
                case PhoneticMatchAlgorithms.NYSSIS:
                    return a.MatchesAsNYSSIS(b);
            }

            return false;
        }

        public static bool Matches(this string a, string b, Func<int,bool> score, ScoreMatchingAlgorithms algorithm)
        {
            if (score == null) return false;
            int scoreValue = 0;
            switch (algorithm)
            {
                case ScoreMatchingAlgorithms.MRA:
                    scoreValue = a.ToMRAScore(b);
                    break;
                case ScoreMatchingAlgorithms.Levenstein:
                    scoreValue = a.ToLevenshtein(b);
                    break;
                case ScoreMatchingAlgorithms.JaroWinker:
                    scoreValue = (int)(a.ToJaroWinklerScore(b) * 100);
                    break;
            }
            return score.Invoke(scoreValue);
        }
    }

    public enum ScoreMatchingAlgorithms
    {
        MRA,Levenstein,JaroWinker
    }
    
    public enum PhoneticMatchAlgorithms
    {
        Soundex,Caverphone,ColognePhonetic,DaitchMokotoff,Metaphone,NYSSIS
    }
    
}