using System;
using System.Collections.Generic;
using System.Linq;
using JCore.Collections;
using PokemonApp.Models;
using PokemonApp.StringSearch;

namespace PokemonApp
{
    public static class PokemonSearchExtensions
    {
        public static IEnumerable<Pokemon> InGeneration(this IEnumerable<Pokemon> pokemon, int index)
        {
            if (index < 0) return pokemon;
            var indexer = pokemon.Filter(x => x.Generation);
            return indexer[index];
        }

        public static IEnumerable<Pokemon> InGeneration(this IEnumerable<Pokemon> pokemon, string generation) => 
            string.IsNullOrWhiteSpace(generation) ? pokemon : pokemon.Filter(x => x.Generation)[generation];

        public static IEnumerable<Pokemon> FromHabitat(this IEnumerable<Pokemon> pokemon, string habitat) => 
            string.IsNullOrWhiteSpace(habitat) ? pokemon : pokemon.Filter(x => x.Habitat)[habitat];

        public static IEnumerable<Pokemon> FromHabitat(this IEnumerable<Pokemon> pokemon, int habitat)
            => pokemon.Filter(x => x.Habitat)[habitat];

        public static IEnumerable<Pokemon> ById(this IEnumerable<Pokemon> pokemon) => pokemon.OrderBy(x => x.Id);
        public static IEnumerable<Pokemon> ByNames(this IEnumerable<Pokemon> pokemon) => pokemon.OrderBy(x => x.Name);

        public static IEnumerable<Pokemon> OrderBy(this IEnumerable<Pokemon> pokemon,PokemonOrder order) => 
            order == PokemonOrder.Id ? pokemon.ById() : pokemon.ByNames();

        public static IEnumerable<Pokemon> SoundsLike(this IEnumerable<Pokemon> pokemon,string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return pokemon;
            return pokemon.Where(x => x.Name.SoundsLike(search));
        }

        public static IEnumerable<Pokemon>
            QueryWith(this IEnumerable<Pokemon> pokemon, Func<Pokemon, string> filter,string query,Func<string,string> beforeTokenProcess = null)
        {
            return string.IsNullOrWhiteSpace(query) ? pokemon : pokemon.Filter(filter).Query(query,beforeTokenProcess);
        }

        public static IEnumerable<Pokemon>
            QueryWith(this IEnumerable<Pokemon> pokemon, Func<Pokemon, string[]> filter,string query,Func<string,string> beforeTokenProcess = null) => 
            string.IsNullOrWhiteSpace(query) ? pokemon : pokemon.Filter(filter).Query(query,beforeTokenProcess);

        public static IEnumerable<Pokemon>
            QueryWith(this IEnumerable<Pokemon> pokemon, Func<Pokemon, string[]> filter,string query,InvertedIndex<string> aliases) => 
            string.IsNullOrWhiteSpace(query) ? pokemon : pokemon.Filter(filter).Query(query,aliases);
        
        public static IEnumerable<Pokemon>
            QueryWith(this IEnumerable<Pokemon> pokemon, Func<Pokemon, string> filter,string query,InvertedIndex<string> aliases) => 
            string.IsNullOrWhiteSpace(query) ? pokemon : pokemon.Filter(filter).Query(query,aliases);
        
    }

    public enum PokemonOrder
    {
        Id,Name
    }
    
}