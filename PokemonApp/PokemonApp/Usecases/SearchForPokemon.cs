using JCore.Application;
using JCore.Application.UseCases;
using JCore.Application.Views;

namespace PokemonApp
{
    public class SearchForPokemon : SearchByString<Pokemon>
    {
        public SearchForPokemon(IQueryFactory<Pokemon> query, StringSearchView<Pokemon> view) : base(query, view)
        {
        }
    }
}