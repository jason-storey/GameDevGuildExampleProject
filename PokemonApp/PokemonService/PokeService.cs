﻿using System.Threading.Tasks;
using PokemonService.Models;
using static ApiModelFactory;
namespace PokemonService
{
    public class PokeService
    {
        PokemonApi _api;
        public PokeService() => _api = new PokemonApi();

        public async Task<Pokemon> Get(int id) => Convert( await _api.GetPokemon(id));
        public async Task<Pokemon> Get(string name) => Convert( await _api.GetPokemon(name));
    }
}