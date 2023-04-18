using System.Collections.Generic;

namespace JCore.Application.Views
{
    public interface StringSearchView<in T> : View
    {
        //Provide
        string Search { get; }
        
        // Receive
        IEnumerable<T> Results { set; }
        IEnumerable<string> AutoComplete { set; }
        string DidYouMean { set; }
    }
}