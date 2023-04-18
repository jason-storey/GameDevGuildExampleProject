using JCore;
using PokemonApp.Factories;
using PokemonApp.Repositories;
namespace PokemonApp
{
    public class Application
    {
        readonly UsecaseFactory _usecases;

        public Application(ViewFactory uis) : this(uis,new PokemonApiRepository()) { }
        public Application(ViewFactory uis,IReadonlyRepository<Pokemon> repository) =>
            _usecases = new UsecaseFactory(new QueryFactory(repository),uis);
        public SearchForPokemon SearchForPokemon() => _usecases.SearchForPokemon();
    }
}