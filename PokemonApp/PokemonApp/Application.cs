using JCore;
using PokemonApp.Factories;
using PokemonApp.Repositories;
using PokemonService;

namespace PokemonApp
{
    public class Application
    {
        readonly UsecaseFactory _usecases;

        public Application(ViewFactory uis) : this(uis,new PokemonServiceRepository(new PokeService())) { }
        public Application(ViewFactory uis,IReadonlyRepository<Pokemon> repository) =>
            _usecases = new UsecaseFactory(new QueryFactory(repository),uis);
        public SearchForPokemon SearchForPokemon() => _usecases.SearchForPokemon();
    }
}