using System;
using System.Collections.Generic;
using System.Security.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonService.Api.Models;

namespace PokemonService
{
    public class PokemonResource
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        
        [JsonProperty("order")]
        public int Order { get; set; }
        
        [JsonProperty("past_types")]
        public List<string> PastTypes { get; set; }

        [JsonProperty("species")]
        public Result Species { get; set; }
        
        [JsonProperty("weight")]
        public float Weight { get; set; }

        [JsonProperty("base_experience")]
        public float BaseExperience { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
        
        [JsonProperty("location_area_encounters")]
        public Uri EncountersResource { get; set; }
        
        [JsonProperty("height")]
        public float Height { get; set; }

        [JsonProperty("held_items")]
        public List<Item> Held { get; set; }

        [JsonProperty("abilities")]
        public List<Ability> Abilities { get; set; }

        [JsonProperty("forms")]
        public List<Result> Forms { get; set; }

        [JsonProperty("types")]
        public List<Types> Type { get; set; }

        [JsonProperty("sprites")]
        public Sprites Sprite { get; set; }
        [JsonProperty("moves")]
        public List<Move> Moves { get; set; }

        [JsonProperty("stats")]
        public List<Stat> Stats { get; set; }
        [JsonProperty("game_indices")]
        public List<Game> Games { get; set; }
        [JsonConverter(typeof(PokemonResourceAbilityConverter))]
        public class Ability
        {
            public string Name;
            public int Slot;
            public bool IsHidden;
            public Uri Uri;
        }

        [JsonConverter(typeof(PokemonResourceItemConverter))]
        public class Item
        {
            public string Name { get; set; }
            public Uri Uri { get; set; }
            public Dictionary<string,int> RarityByGame { get; set; }
        }
        
        [JsonConverter(typeof(PokemonResourceSpriteConverter))]
        public class Sprites
        {
            public Dictionary<string,string> Images { get; set; }
            
        }

        public class Stat
        {
            [JsonProperty("base_stat")]
            public int Base { get; set; }
            [JsonProperty("effort")]
            public int Effort { get; set; }
            [JsonProperty("stat")]
            public Result Details { get; set; }
        }

        public class Types
        {
            [JsonProperty("slot")]
            public int Slot { get; set; }
            [JsonProperty("type")]
            public Result Details { get; set; }
        }
        
        [JsonConverter(typeof(PokemonResourceGameMoveConverter))]
        public class Move
        {
          
            public string Name { get; set; }
       
            public Uri Uri { get; set; }
            public List<MoveGameDetail> Game { get; set; }
        
            public class MoveGameDetail
            {
                public string Name { get; set; }
                public int LearnedAt { get; set; }
                public Uri UriLearnMethod { get; set; }
                public Uri UriVersion { get; set; }
                public string LearnMethod { get; set; }
            }
        }
        
        
    }

    public class PokemonResourceItemConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject d = JObject.Load(reader);
                var item = new PokemonResource.Item();
                item.Name = d["item"]["name"].ToString();
                item.Uri = new Uri(d["item"]["url"].ToString());
                item.RarityByGame = new Dictionary<string, int>();
                foreach (var obj in (JArray)d["version_details"])
                {
                    var name = obj["version"]["name"].ToString();
                    int rarity = int.Parse(obj["rarity"].ToString());
                    item.RarityByGame[name] = rarity;
                }
                return item;
            }
            catch
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(PokemonResource.Item);
    }

    public class PokemonResourceSpriteConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        void AddIfExists(Dictionary<string, Uri> dict,string key,JToken obj)
        {
            try
            {
                if (obj == null || !obj.HasValues) return;
                var uri = new Uri(obj.ToString());
                dict[key] = uri;
            }catch{}
        }

        string ToUri(JToken token)
        {
            if (token == null || !token.HasValues) return null;
            if (token is JProperty p)
            {
                return p.Value.ToString();
            }

            return token.ToString();

        }

        void ProcessProperty(string prefix,Dictionary<string,string> dict,JProperty prop)
        {
            if (prop.Value is JObject obj)
            {
                foreach (var subProp in obj.Properties())
                    ProcessProperty(prefix + prop.Name, dict, subProp);
            }
            else dict[prefix + prop.Name] = ToUri(prop);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            var dict = new Dictionary<string, string>();
            foreach (var property in item.Properties()) 
                ProcessProperty("",dict, property);

            dict["back_default"] = ToUri(item["sprites"]);

            return new PokemonResource.Sprites
            {
                Images = dict
            };
        }


        void AddFromGame(Dictionary<string, Uri> dict, string gen, string game, JToken item)
        {
            try
            {
                AddIfExists(dict, gen+game+"back_default", item["versions"]?[gen]?[game]?["back_default"]);
                AddIfExists(dict, gen+game+"back_gray", item["versions"]?[gen]?[game]?["back_gray"]);
                AddIfExists(dict, gen+game+"back_shiny", item["versions"]?[gen]?[game]?["back_shiny"]);
                AddIfExists(dict, gen+game+"icon_back", item["versions"]?[gen]?[game]?["back_shiny_transparent"]);
                AddIfExists(dict, gen+game+"back_shiny_transparent", item["versions"]?[gen]?[game]?["back_transparent"]);
                AddIfExists(dict, gen+game+"front_default", item["versions"]?[gen]?[game]?["front_default"]);
                AddIfExists(dict, gen+game+"front_shiny", item["versions"]?[gen]?[game]?["front_shiny"]);
                AddIfExists(dict, gen+game+"front_shiny_transparent", item["versions"]?[gen]?[game]?["front_shiny_transparent"]);
                AddIfExists(dict, gen+game+"front_gray", item["versions"]?[gen]?[game]?["front_gray"]);
                AddIfExists(dict, gen+game+"front_transparent", item["versions"]?[gen]?[game]?["front_transparent"]);
            }
            catch
            {
            }
        }
        
        void AddSingleStackGame(Dictionary<string, Uri> dict,string key, JToken item)
        {
            try
            {
                AddIfExists(dict, $"{key}_back_default", item?["back_default"]);
                AddIfExists(dict,$"{key}back_gray" , item?["back_gray"]);
                AddIfExists(dict,$"{key}back_shiny" , item?["back_shiny"]);
                AddIfExists(dict,$"{key}back_shiny_transparent" , item?["back_shiny_transparent"]);
                AddIfExists(dict,$"{key}back_transparent" , item?["back_transparent"]);
                AddIfExists(dict,$"{key}front_default" , item?["front_default"]);
                AddIfExists(dict,$"{key}front_shiny" , item?["front_shiny"]);
                AddIfExists(dict,$"{key}front_shiny_transparent" , item?["front_shiny_transparent"]);
                AddIfExists(dict,$"{key}front_gray" , item?["front_gray"]);
                AddIfExists(dict,$"{key}front_transparent" , item?["front_transparent"]);
            }
            catch
            {
            }
        }
        
        
        public override bool CanConvert(Type objectType) => objectType == typeof(PokemonResource.Sprites);
    }
    
    public class PokemonResourceGameMoveConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
       
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try{
                JObject item = JObject.Load(reader);
                var move = new PokemonResource.Move();

                move.Name = item["move"]["name"].ToString();
                move.Uri = new Uri(item["move"]["url"].ToString());
                move.Game = new List<PokemonResource.Move.MoveGameDetail>();
                foreach (var single in (JArray)item["version_group_details"])
                {
                    var md = new PokemonResource.Move.MoveGameDetail();
                    md.Name = single["version_group"]["name"].ToString();
                    md.LearnedAt = int.Parse(single["level_learned_at"].ToString());
                    md.LearnMethod = single["move_learn_method"]["name"].ToString(); 
                    md.UriLearnMethod = new Uri(single["move_learn_method"]?["url"]?.ToString() ?? string.Empty);
                    md.UriVersion = new Uri(single["version_group"]["url"].ToString());
                    move.Game.Add(md);
                }
                
                return move;
            } catch
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(PokemonResource.Move);
    }
    
    public class PokemonResourceAbilityConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PokemonResource.Ability ability = (PokemonResource.Ability)value;
            writer.WriteStartObject();
            writer.WritePropertyName("ability");
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(ability.Name);
            writer.WritePropertyName("url");
            writer.WriteValue(ability.Uri.ToString());
            writer.WriteEndObject();
            writer.WritePropertyName("is_hidden");
            writer.WriteValue(ability.IsHidden);
            writer.WritePropertyName("slot");
            writer.WriteValue(ability.Slot);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                var abl = new PokemonResource.Ability();
                JObject obj = JObject.Load(reader);
                abl.Name = obj["ability"]?["name"]?.ToString();
                abl.Uri = new Uri(obj["ability"]?["url"]?.ToString() ?? string.Empty);
                abl.IsHidden = (bool)obj["is_hidden"];
                abl.Slot = int.Parse(obj["slot"]?.ToString() ?? string.Empty);
                return abl;
            }
            catch
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(PokemonResource.Ability);
    }
}