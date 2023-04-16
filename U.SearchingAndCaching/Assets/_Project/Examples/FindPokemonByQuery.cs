using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PokemonApp.Models;
using UnityEngine;
namespace JasonStorey
{
    public class FindPokemonByQuery : MonoBehaviour
    {
        [SerializeField]
        TextAsset _saveFile;
        
        Pokemon[] _allPokemon;

        [SerializeField]
        List<Pokemon> _results;
        void Awake()
        {
            _allPokemon = JsonConvert.DeserializeObject<Pokemon[]>(_saveFile.text);
            _results = AllGenOnePokemonWhoLiveInCavesOrAtTheSea().ToList();
        }

        IEnumerable<Pokemon> AllGenOnePokemonWhoLiveInCavesOrAtTheSea() => 
            _allPokemon
                .Filter(x => x.Generation)[0]
                .Filter(x => x.Habitat).Query("cave OR sea");
    }
}
