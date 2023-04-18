using JCore.Application.UseCases;
using JCore.Search;

namespace JCore.Application
{
    public class UsecaseFactory<T>
    {
        readonly ISearchEngine<T> _search;
        readonly IQueryFactory<T> _queries;
        readonly ViewFactory<T> _views;

        public UsecaseFactory(ISearchEngine<T> search,IQueryFactory<T> queries,ViewFactory<T> views)
        {
            _search = search;
            _queries = queries;
            _views = views;
        }

        public SearchByString<T> Search() => new SearchByString<T>(_search,_queries, _views.Searching());
    }
}