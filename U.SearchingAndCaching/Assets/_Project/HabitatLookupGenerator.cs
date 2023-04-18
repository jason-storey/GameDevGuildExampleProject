using System;
using System.Collections.Generic;
using System.Linq;
using JCore.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace JasonStorey
{
    public class HabitatLookupGenerator : MonoBehaviour
    {
        [SerializeField]
        TextAsset _assets;

        [SerializeField]
        List<HabitatAliases> _aliases;

        [SerializeField]
        bool _minified;

        [SerializeField,TextArea(5,50)]
        string _saved;

        [ContextMenu("Save")]
        public void SaveBack()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = _minified ? Formatting.None : Formatting.Indented
            };
            settings.Converters.Add(new InvertedIndex<string>.JsonConverter());
            var asIndex = ToIndex(_aliases).ReverseIndex();
            _saved = JsonConvert.SerializeObject(asIndex,settings);
        }


        [ContextMenu("Load")]
        public void LoadAliases()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new InvertedIndex<string>.JsonConverter());
            var indexes = JsonConvert.DeserializeObject<InvertedIndex<string>>(_assets.text,settings);
            _aliases = ToAliases(indexes.ReverseIndex().ToDictionary().ToIndex());
        }

        
        List<HabitatAliases> ToAliases(InvertedIndex<string> index) => 
            index.ToDictionary().Select(x => HabitatAliases.Create(x.Key, x.Value.ToArray())).ToList();

        InvertedIndex<string> ToIndex(List<HabitatAliases> aliases)
        {
            var ind = new InvertedIndex<string>();
            foreach (var al in aliases) ind.Add(al.Name, al.Aliases.OrderBy(x=>x).ToArray());
            return ind;
        }
    }

    [Serializable]
    public class HabitatAliases
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public List<string> Aliases;

        public static HabitatAliases Create(string name, params string[] aliases) =>
            new()
            {
                Name = name,
                Aliases = aliases.OrderBy(x=>x).ToList()
            };
    }
    
    
}
