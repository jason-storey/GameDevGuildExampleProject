using System;
using JCore;
using JCore.AdvancedSearch;
using JCore.Application;
using JCore.Application.UseCases;
using JCore.Search;
using UnityEngine;

namespace PokemonApp.SimpleSearchApp
{
    public class SearchAppViewBinder
    {
        public SearchAppViewBinder(SearchAppViewer searchAppViewer) => _appViewer = searchAppViewer;
        App<Pokemon> _app;

        public bool AllowEmptySearch
        {
            set => Search.AttemptEmptySearchAnyway = true;
        }
        
        public void PerformSearch() => Search.PerformSearch();
        
        SearchByString<Pokemon> _searcher;
        SearchByString<Pokemon> Search
        {
            get
            {
                if (_searcher != null) return _searcher;
                var search = App.Search();
                return _searcher = search;
            }
        }
        
        ViewFactory<Pokemon> Ui
        {
            get
            {
                var ui = new SimpleViewFactory<Pokemon>();
                RegisterForCallbacks(ui);
                return ui;
            }
        }

        SearchAppViewer _appViewer;
        void RegisterForCallbacks(SimpleViewFactory<Pokemon> ui)
        {
            
            ui.SetResultsDisplay(_appViewer.OnResultsReceived);
            ui.SetSearchProvider(_appViewer.ProvideSearchString);
            ui.SetAutoComplete(_appViewer.ReceivedAutoComplete);
            ui.SetDidYouMean(_appViewer.ReceivedDidYouMean);
            ui.SetMessageHandler(x =>
            {
                OnMessageReceived(x);
                _appViewer.OnMessageReceived(x);
            });
        }
        
        public void DoValidation() => Search.UpdateAutoComplete(); 
        
        void OnMessageReceived(Message message)
        {
            if(message.Is(UseCaseMessages.INTERNAL_ERROR)) Debug.LogException(message.Context as Exception);
            if(message.Is(UseCaseMessages.EMPTY_SEARCH_STRING)) Debug.Log("You didn't type anything to search for!");
            if(message.Is(UseCaseMessages.NO_RESULTS)) Debug.Log("Your search had no results");
            if(message.Is(UseCaseMessages.INVALID_SEARCH_STRING)) Debug.LogWarning("There was a problem with your search query!");
         
        }
        
        PropertySelectors<Pokemon> Properties
        {
            get
            {
                var p = new PropertySelectors<Pokemon>();
                p.Add(x => x.Name);
                return p;
            }
        }

        IReadonlyRepository<Pokemon> Data => _appViewer.ProvideData;
        App<Pokemon> App => _app != null ? _app : new SearchApplicationFactory<Pokemon>(Ui,Data,Properties).CreateApplication();
       
        ISearchEngine<Pokemon> _searchEngine;
    }
}