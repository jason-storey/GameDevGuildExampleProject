using JCore.Application;

namespace PokemonApp.Factories
{
    public class PokemonUsecaseFactory
    {
        readonly IQueryFactory<Pokemon> _queries;
        readonly ViewFactory<Pokemon> _views;

        public PokemonUsecaseFactory(IQueryFactory<Pokemon> queries,ViewFactory<Pokemon> views)
        {
            _queries = queries;
            _views = views;
        }

        public SearchForPokemon SearchForPokemon() => new SearchForPokemon(_queries, _views.Searching());
    }
}