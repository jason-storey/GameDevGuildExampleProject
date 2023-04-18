using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JCore.Application.Presenters;
using JCore.Application.Views;
using static JCore.Application.UseCaseMessages;
namespace JCore.Application.UseCases
{
    public class SearchByString<T> : StringSearchPresenter
    {
        #region plumbing

        readonly IQueryFactory<T> _query;
        readonly StringSearchView<T> _view;

        public SearchByString(IQueryFactory<T> query,StringSearchView<T> view)
        {
            _query = query;
            _view = view;
        }

        public bool AttemptEmptySearchAnyway { get; set; } = true;

        #endregion

        public View View => _view;

        public async void PerformSearch()
        {
            try
            {
                var search = _view.Search;
                if (string.IsNullOrWhiteSpace(search))
                { 
                    Say(EMPTY_SEARCH_STRING);
                    
                    if (AttemptEmptySearchAnyway)
                        await UpdateUiWithResults(string.Empty);
                    else 
                        ClearSearchResults();
                    
                    return;
                }
                
                await UpdateUiWithResults(search);
            }
            catch (Exception ex)
            {
               Say(INTERNAL_ERROR,ex);
            }
        }

        void ClearSearchResults()
        {
            _view.Results = new List<T>();
        }
        
        async Task UpdateUiWithResults(string query)
        {
            var results = (await _query.Search(query).Execute()).ToList();
            if (results.Count <= 0) 
               Say(NO_RESULTS);
            _view.Results = results;
        }

        #region Utility

        void Say(string message, object context = null) => _view.SendMessage(Message.Say(message, context));
        

        #endregion
    }
}