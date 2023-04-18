using Newtonsoft.Json;
using PokemonApp.Pokemon;
using PokemonService;
using UnityEngine;

namespace JasonStorey
{
    public class testthepokemonapi : MonoBehaviour
    {
        [SerializeField] Pokemon _pokemon;

        [SerializeField,Range(0,1000)]
        int _pokemonID = 1;
        
        [SerializeField,TextArea(1,50)]
        string _json;
        [ContextMenu("Loader")]
        async void LoadPokey()
        {
            var pokemon = new PokeService();

            _pokemon = await pokemon.Get(_pokemonID);

            _json = JsonConvert.SerializeObject(_pokemon, Formatting.Indented);
        }
    }
}
