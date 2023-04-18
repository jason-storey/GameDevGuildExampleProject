namespace JCore.Search
{
    public interface ISpellingCorrectionProvider
    {
        void AddRange(params string[] words);
        string CorrectSpelling(string input);
    }
}