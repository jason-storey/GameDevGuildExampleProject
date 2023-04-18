using System;
using System.Collections.Generic;

namespace JCore.Search
{
    public interface IFuzzyMatcher<T>
    {
        IEnumerable<T> FuzzyMatch(IEnumerable<T> items, string searchTerm, Func<T, string> propertySelector, double threshold);
    }
}