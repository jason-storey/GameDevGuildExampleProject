using System;
using System.Collections.Generic;
namespace JCore.Application.Views
{
    public class Ui_Search_Simple<T> : StringSearchView<T>
    {
        readonly Func<string> _provideSearch;
        readonly Action<IEnumerable<T>> _receiveResults;
        readonly Action<Message> _messageHandler;

        public Ui_Search_Simple(Func<string> provideSearch,Action<IEnumerable<T>> receiveResults,Action<Message> messageHandler)
        {
            _provideSearch = provideSearch;
            _receiveResults = receiveResults;
            _messageHandler = messageHandler;
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
    }
}