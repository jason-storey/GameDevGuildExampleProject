using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JCore;
using PokemonService;
using static ApiModelFactory;
using static PokemonApp.ModelFactory;

namespace PokemonApp.Repositories
{
    public class PokemonApiRepository : IReadonlyRepository<Pokemon>
    {
        readonly PokemonApi _api;

        public PokemonApiRepository() : this(new PokemonApi()) { }
        public PokemonApiRepository(PokemonApi api) => _api = api;
        public IEnumerator<Pokemon> GetEnumerator() => GetAll().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Pokemon GetById(string key)
        {
            var task = Task.Run(() => _api.GetPokemon(key));
            task.Wait();
            var toServicePokemon = Convert(task.Result);
            return Convert(toServicePokemon);
        }

        public IEnumerable<Pokemon> GetAll()
        {
            var task = Task.Run(() => _api.GetAllPokemonNames());
            task.Wait();
            int i = 1;
            return task.Result.Select(single => new Pokemon { Id = i++, Name = single }).ToList();
        }


        public void Update(string key, Pokemon item)
        {
        }

        public void Delete(string key)
        {
        }

        public void Add(Pokemon item)
        {
        }
    }
}