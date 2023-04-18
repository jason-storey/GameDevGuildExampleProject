using JCore.Application;

namespace PokemonApp.Factories
{
    public class UsecaseFactory
    {
        readonly IQueryFactory<Pokemon> _queries;
        readonly ViewFactory _views;

        public UsecaseFactory(IQueryFactory<Pokemon> queries,ViewFactory views)
        {
            _queries = queries;
            _views = views;
        }

        public SearchForPokemon SearchForPokemon() => new SearchForPokemon(_queries, _views.SearchForPokemon());
    }
}