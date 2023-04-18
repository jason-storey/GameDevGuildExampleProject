using System;
using System.Collections.Generic;
using System.Linq;
using JCore;
using JCore.Application;
using JCore.Extensions;
using PokemonApp;
using PokemonApp.Factories;
using PokemonApp.Repositories;
using PokemonService;
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
        List<Pokemon> _results;

        [ContextMenu("Search")]
        void PerformSearch() => Search.PerformSearch();

        CoolPokemonApp App => new(Ui, Data);
        
        SearchForPokemon Search
        {
            get
            {
                var search = App.SearchForPokemon();
                search.AttemptEmptySearchAnyway = _allowEmptySearches;
                return search;
            }
        }
        
        ViewFactory<Pokemon> Ui
        {
            get
            {
                var ui = new SimpleViewFactory<Pokemon>();
                ui.SetResultsDisplay(x => _results = x.ToList());
                ui.SetSearchProvider(() => _search);
                ui.SetMessageHandler(OnMessageReceived);
                return ui;
            }
        }

        void OnMessageReceived(Message message)
        {
            if(message.Is(INTERNAL_ERROR)) Debug.LogException(message.Context as Exception);
            if(message.Is(EMPTY_SEARCH_STRING)) Debug.Log("You didn't type anything to search for!");
            if(message.Is(NO_RESULTS)) Debug.Log("Your search had no results");
            if(message.Is(INVALID_SEARCH_STRING)) Debug.LogWarning("There was a problem with your search query!");
        }

        IReadonlyRepository<Pokemon> Data => _useFakeData ? _fakeData.ToRepository() : new PokemonApi().ToRepository();
    }
}
