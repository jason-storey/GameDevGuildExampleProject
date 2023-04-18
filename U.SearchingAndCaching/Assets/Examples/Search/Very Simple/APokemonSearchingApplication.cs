using System;
using System.Collections.Generic;
using System.Linq;
using JCore;
using JCore.Application;
using JCore.Application.UseCases;
using JCore.Extensions;
using JCore.Search;
using PokemonApp;
using PokemonApp.Repositories;
using UnityEngine;
using static JCore.Application.UseCaseMessages;
namespace JasonStorey.Examples.SimpleSearch
{
    public class APokemonSearchingApplication : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        List<Pokemon> _fakeData;

        [Header("Settings")]
        [SerializeField]
        bool _useFakeData;
        [SerializeField]
        bool _allowEmptySearches;

        [Header("Search")]
        [SerializeField]
        string _search;

        [SerializeField]
        List<string> _autoComplete;

        [SerializeField]
        List<Pokemon> _results;

        App<Pokemon> _app;

        [ContextMenu("Search")]
        void PerformSearch() => Search.PerformSearch();

        App<Pokemon> App => _app ??= new App<Pokemon>(Ui, SearchEngine);

        SearchByString<Pokemon> _searcher;
        SearchByString<Pokemon> Search
        {
            get
            {
                if (_searcher != null) return _searcher;
                var search = App.Search();
                search.AttemptEmptySearchAnyway = _allowEmptySearches;
                return _searcher = search;
            }
        }
        
        ViewFactory<Pokemon> Ui
        {
            get
            {
                var ui = new SimpleViewFactory<Pokemon>();
                ui.SetResultsDisplay(x => _results = x.ToList());
                ui.SetSearchProvider(() => _search);
                ui.SetAutoComplete(x=>_autoComplete = x.ToList());
                ui.SetMessageHandler(OnMessageReceived);
                return ui;
            }
        }

        void OnValidate() => Search.UpdateAutoComplete();

        void OnMessageReceived(Message message)
        {
            if(message.Is(INTERNAL_ERROR)) Debug.LogException(message.Context as Exception);
            if(message.Is(EMPTY_SEARCH_STRING)) Debug.Log("You didn't type anything to search for!");
            if(message.Is(NO_RESULTS)) Debug.Log("Your search had no results");
            if(message.Is(INVALID_SEARCH_STRING)) Debug.LogWarning("There was a problem with your search query!");
        }

        IReadonlyRepository<Pokemon> Data => _useFakeData ? _fakeData.ToRepository() : new PokemonApiRepository();
        ISearchEngine<Pokemon> SearchEngine
        {
            get
            {
                if (_searchEngine != null) return _searchEngine;
                var autocomplete = new Autocomplete<Pokemon>();
                autocomplete.AddSelector(x=>x.Name);
                autocomplete.AddRange(Data);
                return _searchEngine = new SearchEngineV1<Pokemon>(Data,autocomplete);
            }
        }

        ISearchEngine<Pokemon> _searchEngine;
    }
}
