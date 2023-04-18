using System.Linq;

namespace JCore.Search
{
    public class BKLevensteinSpellingCorrector : ISpellingCorrectionProvider
    {
        BKTree _tree;
        public BKLevensteinSpellingCorrector() => _tree = new BKTree();

        public void AddRange(params string[] words)
        {
            foreach (var word in words) 
                _tree.Insert(word);
        }
        
        public string CorrectSpelling(string input) => _tree.Query(input, 2).FirstOrDefault() ?? input;
    }
}