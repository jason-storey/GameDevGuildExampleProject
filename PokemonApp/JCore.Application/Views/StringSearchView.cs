using System.Collections.Generic;

namespace JCore.Application.Views
{
    public interface StringSearchView<in T> : View
    {
        string Search { get; }

        IEnumerable<T> Results { set; }

    }
}