using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JCore.Search;

namespace JCore.Application.Queries
{
    public class UseSearchEngineByPredicate<T> : IQuery<T>
    {
        readonly ISearchEngine<T> _engine;
        readonly Func<T, bool> _predicate;

        public UseSearchEngineByPredicate(ISearchEngine<T> engine,Func<T,bool> predicate)
        {
            _engine = engine;
            _predicate = predicate;
        }

        public Task<IEnumerable<T>> Execute() => Task.FromResult(_engine.Search(_predicate));
    }
}