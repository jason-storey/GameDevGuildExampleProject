namespace PokemonApp
{
    public static class ModelFactory
    {
        public static Pokemon Convert(PokemonService.Models.Pokemon pokemon)
        {
            if (pokemon == null) return null;
            var appPokemon = new Pokemon
            {
                Id = pokemon.Id,
                Name = pokemon.Name
            };
            return appPokemon;
        }
    }
}