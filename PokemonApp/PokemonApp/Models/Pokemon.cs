using System;

namespace PokemonApp
{
    [Serializable]
    public class Pokemon
    {
        public string Name;
        public int Id;
        public string[] Types;
        public float Weight { get; set; }
        public float Height { get; set; }
    }
}