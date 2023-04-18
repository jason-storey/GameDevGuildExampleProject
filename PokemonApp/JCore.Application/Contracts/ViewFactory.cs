
using JCore.Application.Views;

namespace PokemonApp.Factories
{
    public interface ViewFactory<in T>
    {
        StringSearchView<T> Searching();
    }
}