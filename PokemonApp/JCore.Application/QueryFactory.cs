using System;
using JCore.AdvancedSearch;
using JCore.Application.Queries;

namespace JCore.Application
{
    public class QueryFactory<T> : IQueryFactory<T>
    {
        public IQuery<T> Search(string query) => new UseSearchEngineByString<T>(_engine, query);
  
        readonly ISearchEngine<T> _engine;
        public QueryFactory(ISearchEngine<T> engine) => _engine = engine;
    }

}