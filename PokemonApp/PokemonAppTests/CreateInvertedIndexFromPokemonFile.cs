using System;
using System.IO;
using System.Linq;
using JCore.Collections;
using Newtonsoft.Json;
using PokemonApp.Models;

namespace PokemonApp
{
    public static class CreateInvertedIndexFromPokemonFileUsingConverter
    {
        public static InvertedIndex<Pokemon> Create(string filePath = "Data/pokemon.json")
        {
            var entries = (JsonConvert.DeserializeObject<Pokemon[]>(File.ReadAllText(filePath)) ?? Array.Empty<Pokemon>()).Where(x=>x != null);
            return entries.Filter(x => x.Types);
        }
    }
    
}