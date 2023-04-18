using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCore.Application.Queries
{
    public class SearchARepositoryByString<T> : IQuery<T>
    {
        readonly IReadonlyRepository<T> _repository;
        readonly string _query;

        public SearchARepositoryByString(IReadonlyRepository<T> repository,string query)
        {
            _repository = repository;
            _query = query;
        }

        public async Task<IEnumerable<T>> Execute() => _repository;
    }
    

}