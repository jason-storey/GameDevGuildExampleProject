using ApiPokemon = PokemonService.Models.Pokemon;
using UnityPokemon = PokemonApp.Pokemon;
namespace PokemonApp
{
    public static class ModelFactory
    {
        public static UnityPokemon Convert(ApiPokemon pokemon)
        {
            if (pokemon == null) return null;
            var appPokemon = new UnityPokemon
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Types = pokemon.Types,
                Weight = pokemon.Weight,
                Height = pokemon.Height
            };
            return appPokemon;
        }
    }
}