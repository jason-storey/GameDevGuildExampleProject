using JCore.Application.Queries;

namespace JCore.Application
{
    public class QueryFactory<T> : IQueryFactory<T>
    {
        public IQuery<T> Search(string keyword) => new SearchARepositoryByString<T>(_repo, keyword);
        
        readonly IReadonlyRepository<T> _repo;
        public QueryFactory(IReadonlyRepository<T> repo) => _repo = repo;
    }
}