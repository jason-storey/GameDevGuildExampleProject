using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace PokemonApp.Models
{
    [Serializable,JsonConverter(typeof(PokemonConverter))]
    public class Pokemon
    {
        public string Name;
        public int Id;
        public float Height;
        public float Weight;
        public string[] Types;
        public string[] Images;
        public int CaptureRate;
        public string Generation;
        public string Habitat;
        public Description[] Descriptions;
        public bool Legendary;
    }

    [Serializable]
    public class PokemonSummary
    {
        public string Name;
        public int Id;
    }
     [Serializable]
    public class Description
    {
        public string Game;
        [SerializeField,TextArea(1,30)]
        public string Text;
    }

    public class PokemonConverter : JsonConverter
    {
        const string IMAGE_BASE_URL = "https://raw.githubusercontent.com/PokeAPI/sprites/master/";
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var pokemon = (Pokemon)value;
     
            writer.WriteStartObject();
        
            writer.WritePropertyName("id");
            writer.WriteValue(pokemon.Id);
        
            writer.WritePropertyName("name");
            writer.WriteValue(pokemon.Name);
        
            writer.WritePropertyName("gen");
            writer.WriteValue(pokemon.Generation);

            writer.WritePropertyName("types");
            serializer.Serialize(writer, pokemon.Types);
        
            writer.WritePropertyName("legendary");
            writer.WriteValue(pokemon.Legendary);
        
            writer.WritePropertyName("habitat");
            writer.WriteValue(pokemon.Habitat);
        
            writer.WritePropertyName("height");
            writer.WriteValue(pokemon.Height);
        
            writer.WritePropertyName("weight");
            writer.WriteValue(pokemon.Weight);
        
            writer.WritePropertyName("captureRate");
            writer.WriteValue(pokemon.CaptureRate);
        
            writer.WritePropertyName("images");
            serializer.Serialize(writer, pokemon.Images.Select(x=>x.Replace(IMAGE_BASE_URL,"").Trim()));
            
            writer.WritePropertyName("descriptions");
            writer.WriteStartObject();
            foreach (var desc in pokemon.Descriptions)
            {
                writer.WritePropertyName(desc.Game);
                writer.WriteValue(desc.Text);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var pokemon = new Pokemon();

            try
            {
                if (reader == null) return null;
                var jsonObject = JObject.Load(reader);
                if (!jsonObject.HasValues) return null;
                pokemon.Id = jsonObject["id"].Value<int>();
                pokemon.Name = jsonObject["name"].Value<string>();
                pokemon.Generation = jsonObject["gen"].Value<string>();
                pokemon.Types = jsonObject["types"].ToObject<string[]>();
                pokemon.Legendary = jsonObject["legendary"].Value<bool>();
                pokemon.Habitat = jsonObject["habitat"].Value<string>();
                pokemon.Height = jsonObject["height"].Value<float>();
                pokemon.Weight = jsonObject["weight"].Value<float>();
                pokemon.CaptureRate = jsonObject["captureRate"].Value<int>();
                pokemon.Images = jsonObject["images"].ToObject<string[]>().Select(x =>x.Replace(IMAGE_BASE_URL,""))
                    .ToArray();

                var descObject = (JObject)jsonObject["descriptions"];
                pokemon.Descriptions = (descObject.Properties()
                    .Select(property => new { property, game = property.Name })
                    .Select(@t => new { @t, text = t.property.Value.ToString() })
                    .Select(@t => new Description { Game = @t.@t.game, Text = @t.text })).ToArray();
                return pokemon;
            }
            catch
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Pokemon);
    }
    
}