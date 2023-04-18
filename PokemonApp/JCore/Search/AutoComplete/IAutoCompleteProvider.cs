using System.Collections.Generic;

namespace JCore.Search
{
    public interface IAutoCompleteProvider<in T>
    {
        IEnumerable<string> GetSuggestions(string input);
        void AddRange(params T[] items);
    }
}