using System.Linq;
using PokemonApp;
using PokemonApp.Repositories;
using UnityEngine;

namespace JasonStorey
{
    public class RealDataSearchForAPokemon : MonoBehaviour
    {
        [SerializeField,Range(1,1300)]
        int _id = 1;

        [SerializeField]
        Pokemon[] _results;


        [SerializeField]
        Pokemon _single;
        
        [ContextMenu("Search")]
        void PerformSearch()
        {
            var api = new PokemonApiRepository();
            _results = api.GetAll().ToArray();
            _single = api.GetById("110");
        }
    }
}
