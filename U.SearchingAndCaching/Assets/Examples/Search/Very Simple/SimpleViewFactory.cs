using System;
using System.Collections.Generic;
using JCore.Application;
using JCore.Application.Views;
namespace JasonStorey.Examples.SimpleSearch
{
    public class SimpleViewFactory<T> : ViewFactory<T>
    {
        Func<string> _search;
        public void SetSearchProvider(Func<string> search) => _search = search;
        Action<IEnumerable<T>> _results;
        public void SetResultsDisplay(Action<IEnumerable<T>> results) => _results = results;

        public void SetAutoComplete(Action<IEnumerable<string>> autoComplete) => _autoComplete = autoComplete;
        
        Action<Message> _messages;
        Action<IEnumerable<string>> _autoComplete;
        public void SetMessageHandler(Action<Message> message) => _messages = message; 
        
        public StringSearchView<T> Searching() => new Ui_Search_Simple<T>(_search,_autoComplete,_results,_messages);
    }
}