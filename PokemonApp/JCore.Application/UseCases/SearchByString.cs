using System;
using JCore.Application.Presenters;
using JCore.Application.Views;
using static JCore.Application.MessageUtilities;

namespace JCore.Application.UseCases
{
    public class SearchByString<T> : StringSearchPresenter
    {
        readonly IQueryFactory<T> _queryFactory;
        readonly StringSearchView<T> _view;

        public SearchByString(IQueryFactory<T> queryFactory,StringSearchView<T> view)
        {
            _queryFactory = queryFactory;
            _view = view;
        }
        

        public async void PerformSearch()
        {
            try
            {
                _view.Results = await _queryFactory.Search(_view.Search).Execute();
            }
            catch (Exception ex)
            {
                _view.SendMessage(FromError(ex));
            }
        }
    }
}