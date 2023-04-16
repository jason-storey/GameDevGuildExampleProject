using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
namespace PokemonApp
{
    public static class CreateInvertedIndexFromPokemonFile
    {
        public static InvertedIndex<Pokemon> Create(string filePath = "Data/pokemon.json")
        {
            var _fails = new List<Exception>();
            var index = new InvertedIndex<Pokemon>();
            var jsonString = File.ReadAllText(filePath);
            var read = JArray.Parse(jsonString);
            foreach (var entry in read)
            {
                try
                {
                    if (entry == null || !entry.HasValues) continue;
                    var p = new Pokemon
                    {
                        Id = int.Parse((string)entry["id"] ?? "-1"),
                        Name = entry["name"]?.ToString(),
                        Types = entry["types"]?.ToObject<string[]>()
                    };
                    index.Add(p, p.Types ?? Array.Empty<string>());
                }
                catch (Exception ex)
                {
                    _fails.Add(ex);
                }
            }
            return index;
        }
    }

    [Serializable]
    public class Pokemon
    {
        public int Id;
        public string Name;
        public string[] Types;
    }
}