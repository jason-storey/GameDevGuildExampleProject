using JCore;
using PokemonApp.Factories;
using PokemonApp.Repositories;
namespace PokemonApp
{
    public class CoolPokemonApp
    {
        readonly PokemonUsecaseFactory _usecases;

        public CoolPokemonApp(ViewFactory<Pokemon> uis) : this(uis,new PokemonApiRepository()) { }
        public CoolPokemonApp(ViewFactory<Pokemon> uis,IReadonlyRepository<Pokemon> repository) =>
            _usecases = new PokemonUsecaseFactory(new QueryFactory(repository),uis);
        
        public SearchForPokemon SearchForPokemon() => _usecases.SearchForPokemon();
    }
}