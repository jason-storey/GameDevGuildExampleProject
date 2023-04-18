/**
using System.Collections.Generic;
using System.Linq;
using JCore;
using JCore.Collections;
using Newtonsoft.Json;
using UnityEngine;
namespace JasonStorey
{
    public class FindPokemonByQuery : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        TextAsset _saveFile;
        [SerializeField]
        TextAsset _habitatSynonyms;
        
        [Header("Natural Search")]
        [SerializeField]
        string _named = "";
        [SerializeField]
        string types = "";
        [SerializeField]
        string habitat;
        
        [Header("Filters")]
        [SerializeField,Range(-1,8)]
        int _generation = 0;

        [Header("Ordering")]
        [SerializeField]
        PokemonOrder _order;

        [Header("Pagination")] 
        [SerializeField]
        bool _paginate = false;
        [SerializeField,Range(5,100)]
        int _pageSize = 50;
        [SerializeField,Range(1,30)]
        int _page = 0;
        
        [Header("Results")]
        [SerializeField,TextArea(2,4)]
        string _aboutResults;

        [SerializeField]
        List<string> _results;
        void Awake()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new InvertedIndex<string>.JsonConverter());
            _habitatAlternatives = JsonConvert.DeserializeObject<InvertedIndex<string>>(_habitatSynonyms.text,settings);
            _allPokemon = JsonConvert.DeserializeObject<Pokemon[]>(_saveFile.text);
            _generations = _allPokemon.Filter(x => x.Generation);
            RefreshResults();
        }

        void OnValidate()
        {
            if (!Application.isPlaying) return;
            if (_generations == null) return;
            
            RefreshResults();
            ClampPageControlsInEditor();
        }

        void RefreshResults()
        {
            _allResults = GetResults();
            _results = Names;
            WriteReadoutMessage();
        }

        void WriteReadoutMessage()
        {
            _aboutResults = $"{_allResults.Count}.";
            if (_generation > -1)
                _aboutResults += $"(Gen {_generation+1})";
            else _aboutResults += "(All Generations)";
            if (!_paginate)
                return;
            if (_page == 0) _aboutResults += " (Showing All)";
            else _aboutResults += $"Results in {_totalPages} Pages. ({_pageSize} per page.) ({_page+1}/{_totalPages})";
        }

        void ClampPageControlsInEditor()
        {
            if (!_paginate) return;
            if (_page <= 0) return;
            if (_pageSize > _allResults.Count) _pageSize = _allResults.Count;
            _totalPages = _allResults.GetPageCount(_pageSize);
            if (_page >= _totalPages) _page = _totalPages-1;
        }
/*
        List<Pokemon> GetResults() =>
            _allPokemon
                .InGeneration(_generation)
                .QueryWith(x=>x.Types,types)
                .SoundsLike(_named)
                .QueryWith(x=>x.Habitat,habitat,_habitatAlternatives)
                .OrderBy(_order).ToList();
     
        IEnumerable<Pokemon> Paginated => !_paginate ? _allResults : _allResults.Paginated(_pageSize, _page);

        List<string> Names => Paginated.Select(x => $"{x.Id} - {x.Name}").ToList();

        int _totalPages;
        List<Pokemon> _allResults;
        Pokemon[] _allPokemon;
        InvertedIndex<Pokemon> _generations;
        InvertedIndex<string> _habitatAlternatives;
    }


    
    
}
    **/