using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonService.Api.Models
{
    public class ResourceList
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
        

    }
    public class Result
    {
        public string name { get; set; }
        public Uri url { get; set; }
    }

    
    public class Game
    {
        [JsonProperty("game_index")]
        public int Id { get; set; }
        [JsonProperty("version")]
        public Result Version { get; set; }
    }
}