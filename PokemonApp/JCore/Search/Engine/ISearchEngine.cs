using System;
using System.Collections.Generic;

namespace JCore.Search
{
    public interface ISearchEngine<T>
    {
        IEnumerable<T> Search(Func<T, bool> predicate);

        void AddRange(params T[] items);

        IEnumerable<string> AutoComplete(string query);
        IEnumerable<T> Search(string query);
        bool TrySpellcheck(string search, out string suggestion);
    }
}