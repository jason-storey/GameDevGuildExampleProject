using System.Collections.Generic;
using System.Threading.Tasks;
using JCore.Search;

namespace JCore.Application.Queries
{
    public class UseSearchEngineByString<T> : IQuery<T>
    {
        readonly ISearchEngine<T> _engine;
        readonly string _query;

        public UseSearchEngineByString(ISearchEngine<T> engine,string query)
        {
            _engine = engine;
            _query = query;
        }

        public async Task<IEnumerable<T>> Execute()
        {
            return _engine.Search(_query);
        }
    }
}