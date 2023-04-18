using System;
using System.Collections.Generic;
using System.Linq;
using PokemonApp.Pokemon;
using PokemonService;
namespace PokemonApp.Pokemon
{
    [Serializable]
    public class Pokemon
    {
        public string Name;
        public int Id;
        public int Order;
        public string Species;
        public float Height;
        public float Weight;
        public float BaseExperience;
        public bool IsDefault;
        public string[] PastTypes;
        public string[] Types;
        public string[] Abilities;
        public string[] Forms;
        public PokemonStat[] Stats;
        public GameSpecific[] Games;
        public PokemonImage[] Images;
    }
    }

[Serializable]
public class GameSpecific
{
    public string Name;
    public List<Move> Moves;
    public List<Item> Items;
}

[Serializable]
public class Move
{
    public string Name;
    public int LearnedAtLevel;
    public string LearnedBy;
}

[Serializable]
public class Item
{
    public string Name;
    public float Rarity;
}

[Serializable]
public class PokemonImage
{
    public string Key;
    public string Url;
}

[Serializable]
public class PokemonStat
{
    public string Name;
    public int Base;
    public int Effort;
}

public static class ModelFactory
    {
        public static Pokemon Convert(PokemonResource p)
        {
            if (p == null) return null;
            var GamesNames = p.Games.Select(x => x.Version.name).OrderBy(x => x).Where(x => x != null).ToArray();
            var pk = new Pokemon
            {
                Order = p.Order,
                Name = p.Name,
                Id = p.Id,
                Abilities = p.Abilities.Select(x => x.Name).OrderBy(x => x).ToArray(),
                Forms = p.Forms.Select(x => x.name).OrderBy(x => x).ToArray(),
              
                Height = p.Height,
                Species = p.Species.name,
Images = p.Sprite.Images.Select(x=>new PokemonImage
{
    Key = x.Key,
    Url = x.Value
}).ToArray()
            };
            pk.Stats = p.Stats.Select(x => new PokemonStat
            {
                Base = x.Base,
                Effort = x.Effort,
                Name = x.Details.name
                
            }).ToArray();
            Dictionary<string, GameSpecific> allGames = new Dictionary<string, GameSpecific>();

            foreach (var gamesName in GamesNames)
            {
                allGames[gamesName] = CreateEntry();
            }
            //  pk.MoveNames = p.Moves.Select(x => x.Name).ToArray();
            foreach (var m in p.Moves)
            {
                foreach (var gameSpecific in m.Game)
                {
                    if (!allGames.ContainsKey(gameSpecific.Name)) allGames[gameSpecific.Name] = CreateEntry();
                    var g = allGames[gameSpecific.Name];
                    var newMove = new Move
                    {
                        Name = m.Name,
                        LearnedBy = gameSpecific.LearnMethod,
                        LearnedAtLevel = gameSpecific.LearnedAt
                    };
                    if (g.Moves == null) g.Moves = new List<Move>();
                    g.Moves.Add(newMove);
                    g.Name = gameSpecific.Name;
                }
            }

            foreach (var heldItem in p.Held)
            {
                foreach (var kvp in heldItem.RarityByGame)
                {
                    var gameName = kvp.Key;
                    var rarity = kvp.Value;

                    if (!allGames.ContainsKey(gameName)) allGames[gameName] = CreateEntry();
                    if (allGames[gameName].Items == null) allGames[gameName].Items = new List<Item>();
                    allGames[gameName].Items.Add(new Item
                    {
                        Name = heldItem.Name,
                        Rarity = rarity
                    });
                    
                }
            }

            pk.Types = p.Type.OrderBy(x => x.Slot).Select(x => x.Details.name).ToArray();
            pk.Weight = p.Weight;
            pk.BaseExperience = p.BaseExperience;
            pk.IsDefault = p.IsDefault;
            pk.PastTypes = p.PastTypes.ToArray();
            pk.Games = allGames.Values.Where(x=>x.Name != null).ToArray();
            return pk;
        }

        static GameSpecific CreateEntry()
        {
            return new GameSpecific
            {
               Items =  new List<Item>(),
                Moves = new List<Move>()
            };
        }
    }
    