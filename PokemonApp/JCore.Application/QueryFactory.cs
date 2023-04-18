using System;
using JCore.Application.Queries;
using JCore.Search;

namespace JCore.Application
{
    public class QueryFactory<T> : IQueryFactory<T>
    {
        public IQuery<T> Search(string query) => new UseSearchEngineByString<T>(_engine, query);
        public IQuery<T> Search(Func<T, bool> predicate) => new UseSearchEngineByPredicate<T>(_engine, predicate);

        readonly ISearchEngine<T> _engine;
        public QueryFactory(ISearchEngine<T> engine)
        {
            _engine = engine;
        }
    }
}