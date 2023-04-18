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

#pragma warning disable CS1998
        public async Task<IEnumerable<T>> Execute() => new List<T> {_repository.GetById(_query)};
#pragma warning restore CS1998
    }
}