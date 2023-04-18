using System.Collections.Generic;
using System.Linq;
using JCore;
using JCore.Application;
using JCore.Extensions;
using PokemonApp;
using PokemonApp.Repositories;
using PokemonApp.SimpleSearchApp;
using UnityEngine;

namespace JasonStorey.Examples.SimpleSearch
{
    public class APokemonSearchingApplication : MonoBehaviour,SearchAppViewer
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
        string _didYouMean;
        
        [SerializeField]
        List<Pokemon> _results;
        
        [ContextMenu("Search")]
        public void DoSearch() => App.PerformSearch();
        void OnValidate() => App.DoValidation();
        
        #region Binding
        
        SearchAppViewBinder App => _application ??= new SearchAppViewBinder(this);
        public void OnResultsReceived(IEnumerable<Pokemon> results) => _results = results.ToList();
        public string ProvideSearchString() => _search;
        public void ReceivedAutoComplete(IEnumerable<string> autoComplete) => _autoComplete = autoComplete.ToList();
        public void ReceivedDidYouMean(string alternativeSpelling) => _didYouMean = alternativeSpelling;
        public void OnMessageReceived(Message message) { }
        public IReadonlyRepository<Pokemon> ProvideData =>_useFakeData ? _fakeData.ToRepository() : new PokemonApiRepository();
        
        SearchAppViewBinder _application;
        
        #endregion
    }
}
