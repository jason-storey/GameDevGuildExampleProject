using System.Collections.Generic;
using JCore;
using JCore.Application;

namespace PokemonApp.SimpleSearchApp
{
    public interface SearchAppViewer
    {
        void OnResultsReceived(IEnumerable<Pokemon> results);
        string ProvideSearchString();
        void ReceivedAutoComplete(IEnumerable<string> autoComplete);
        void ReceivedDidYouMean(string alternativeSpelling);
        void OnMessageReceived(Message message);
        IReadonlyRepository<Pokemon> ProvideData { get; }
    }
}