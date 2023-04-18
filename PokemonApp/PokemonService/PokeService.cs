using System.Threading.Tasks;
using PokemonApp.Pokemon;

namespace PokemonService
{
    public class PokeService
    {
        PokemonApi _api;
        public PokeService() => _api = new PokemonApi();

        public async Task<Pokemon> Get(int id)
        {
            var result = await _api.GetPokemon(id);
            return ModelFactory.Convert(result);
        }
        
    }
}