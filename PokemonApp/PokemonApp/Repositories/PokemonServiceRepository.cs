using System.Collections;
using System.Collections.Generic;
using JCore;
using PokemonService;
using static PokemonApp.ModelFactory;

namespace PokemonApp.Repositories
{
    public class PokemonServiceRepository : IReadonlyRepository<Pokemon>
    {
        readonly PokeService _service;

        public PokemonServiceRepository(PokeService service) => _service = service;
        public int Amount { get; set; } = 5;
        
        public IEnumerator<Pokemon> GetEnumerator() => GetAll().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Pokemon GetById(string key)
        {
            var single = _service.Get(key).Result;
            return Convert(single);
        }

        public IEnumerable<Pokemon> GetAll()
        {
            for (int i = 0; i < Amount; i++)
            {
                var result = _service.Get(i.ToString()).Result;
                yield return Convert(result);
            }
        }
        
    }
}