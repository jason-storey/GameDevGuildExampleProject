using JCore.Application.Views;

namespace PokemonApp.Factories
{
    public interface ViewFactory
    {
        StringSearchView<Pokemon> SearchForPokemon();
    }
}