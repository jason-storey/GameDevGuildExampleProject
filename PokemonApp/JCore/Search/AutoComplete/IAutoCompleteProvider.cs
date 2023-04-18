using System.Collections.Generic;

namespace JCore.Search
{
    public interface IAutoCompleteProvider<T>
    {
        IEnumerable<string> GetSuggestions(string input);
    }
}