using System;
using System.Collections.Generic;
using System.Linq;
using JCore;
using JCore.Application;
using JCore.Application.Views;
using JCore.Extensions;
using PokemonApp;
using PokemonApp.Factories;
using UnityEngine;
using Application = PokemonApp.Application;
namespace JasonStorey.Examples.SimpleSearch
{
    public class SimpleSearchForAPokemon : MonoBehaviour
    {
        [SerializeField]
        List<Pokemon> _available;

        [SerializeField]
        string _search;

        [SerializeField]
        List<Pokemon> _results;

        [ContextMenu("Search")]
        void PerformSearch()
        {
            var app = new Application(Ui, _available.ToRepository());
           app.SearchForPokemon().PerformSearch();
        }

        #region Plumbing
        Views Ui => new(() => _search, r => _results = r.ToList());
        
        #endregion
    }

    #region Inline Invisible UI and Interface
    
    public class Views : ViewFactory
    {
        readonly SearchUi _ui;
        public Views(Func<string> search,Action<IEnumerable<Pokemon>> results) => _ui = new SearchUi(search, results);
        public StringSearchView<Pokemon> SearchForPokemon() => _ui;
    }

    public class SearchUi : StringSearchView<Pokemon>
    {
        readonly Func<string> _provideSearch;
        readonly Action<IEnumerable<Pokemon>> _receiveResults;

        public SearchUi(Func<string> provideSearch,Action<IEnumerable<Pokemon>> receiveResults)
        {
            _provideSearch = provideSearch;
            _receiveResults = receiveResults;
        }
        
        public void SendMessage(Message message) => Debug.Log(message);
        public bool IsBusy { get; set; }
        public string Search => _provideSearch?.Invoke();

        public IEnumerable<Pokemon> Results
        {
            set =>_receiveResults?.Invoke(value);
        } 
    }
    
    #endregion
}