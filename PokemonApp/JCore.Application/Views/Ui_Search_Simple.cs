using System;
using System.Collections.Generic;
namespace JCore.Application.Views
{
    public class Ui_Search_Simple<T> : StringSearchView<T>
    {
        readonly Func<string> _provideSearch;
        readonly Action<IEnumerable<string>> _autoComplete;
        readonly Action<IEnumerable<T>> _receiveResults;
        readonly Action<Message> _messageHandler;
        readonly Action<string> _didyoumean;

        public Ui_Search_Simple(
            Func<string> provideSearch,
            Action<IEnumerable<string>> autoComplete,
            Action<IEnumerable<T>> receiveResults,
            Action<Message> messageHandler,Action<string> didyoumean)
        {
            _provideSearch = provideSearch;
            _autoComplete = autoComplete;
            _receiveResults = receiveResults;
            _messageHandler = messageHandler;
            _didyoumean = didyoumean;
        }
        public void SendMessage(Message message) => _messageHandler?.Invoke(message);
        public bool ShowErrorDetails { get; set; }

        public bool IsBusy { get; set; }
        public string Search => _provideSearch?.Invoke();

        public IEnumerable<T> Results
        {
            set => _receiveResults?.Invoke(value);
        }

        public bool ClearSearchAfterResultsReturned { get; set; }
        public IEnumerable<string> AutoComplete
        {
            set => _autoComplete?.Invoke(value);
        }

        public string DidYouMean
        {
            set => _didyoumean.Invoke(value);
        }
    }
}