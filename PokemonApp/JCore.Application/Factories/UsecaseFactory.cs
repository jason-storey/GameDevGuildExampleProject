using JCore.Application.UseCases;

namespace JCore.Application
{
    public class UsecaseFactory<T>
    {
        readonly IQueryFactory<T> _queries;
        readonly ViewFactory<T> _views;

        public UsecaseFactory(IQueryFactory<T> queries,ViewFactory<T> views)
        {
            _queries = queries;
            _views = views;
        }

        public SearchByString<T> Search() => new SearchByString<T>(_queries, _views.Searching());
    }
}