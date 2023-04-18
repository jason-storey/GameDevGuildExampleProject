using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonService.Api.Models;
namespace PokemonService
{
    public class PokemonApi
    {
        protected virtual void OnRequestSent(string url, HttpResponseMessage responses) => RequestReceived?.Invoke(url, responses);
        public event Action<string, HttpResponseMessage> RequestReceived;


        public PokemonApi()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BASE);
        }

        public async Task<Dictionary<string, Uri>> GetResources(string url, int limit = 20, int offset = 20)
        {
            var list = await GetResourceList($"{url}?limit={limit}&offset={offset}");
            return list.results.ToDictionary(
                x => x.name,
                x => x.url);
        }
        public async Task<ResourceList> GetResourceList(string url,int limit = 20, int offset = 20)
        {
            var json = await GetJson(url);
            return JsonConvert.DeserializeObject<ResourceList>(json);
        }

        
        #region plumbing
        
        public async Task<Dictionary<string, Uri>> AllResources()
        {
            try
            {
                var json = await GetJson(string.Empty);
                JObject jo = JObject.Parse(json);
                Dictionary<string, Uri> resources = new Dictionary<string, Uri>();
                foreach (var property in jo.Properties())
                {
                    try
                    {
                      resources.Add(property.Name,new Uri(property.Value.ToString()));
                    }
                    catch { }
                }

                return resources;
            }
            catch
            {
                return new Dictionary<string, Uri>();
            }
        }

        public async Task<string> GetJson(string url)
        {
            try
            {
                var result = await _client.GetAsync(url);
                OnRequestSent(url, result);
                return await result.Content.ReadAsStringAsync();
            }
            catch
            {
                return string.Empty;
            }
        }


        public void Dispose() => _client?.Dispose();
        readonly HttpClient _client;
        const string BASE = "https://pokeapi.co/api/v2/";
        const string POKEMON = "pokemon";

        #endregion
        
        public async Task<PokemonResource> GetPokemon(int i) => 
            JsonConvert.DeserializeObject<PokemonResource>(await GetJson($"pokemon/{i}"));
        public async Task<PokemonResource> GetPokemon(string name) => 
            JsonConvert.DeserializeObject<PokemonResource>(await GetJson($"pokemon/{name}"));
    }
}
