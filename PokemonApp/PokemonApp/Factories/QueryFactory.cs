using JCore;
using JCore.Application;
using JCore.Application.Queries;

namespace PokemonApp.Factories
{
    public class QueryFactory : IQueryFactory<Pokemon>
    {
        public IQuery<Pokemon> Search(string keyword) => new SearchARepositoryByString<Pokemon>(_pokemon, keyword);
        
        readonly IReadonlyRepository<Pokemon> _pokemon;
        public QueryFactory(IReadonlyRepository<Pokemon> pokemon) => _pokemon = pokemon;
    }
}