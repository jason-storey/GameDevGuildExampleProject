using System;
using System.Collections.Generic;
using System.Linq;
using JCore.Extensions;
using JCore.Search;
using Newtonsoft.Json;
using UnityEngine;

namespace JasonStorey
{
    public class ExperimentingWithSearchEngine : MonoBehaviour
    {
        [SerializeField]
        string _dataString;
        
        [SerializeField]
        List<Data> _data;

        [SerializeField]
        string _query;

        [SerializeField]
        List<string> _autoComplete;
        
        [SerializeField]
        List<Data> _results;

        void Start()
        {
            var repo = _data.ToRepository();
            var autoComplete = new Autocomplete<Data>();
            autoComplete.AddSelector(x=>x.Name);
            autoComplete.AddRange(repo);
            _engine = new SearchEngineV1<Data>(repo,autoComplete);
        }

        void Update()
        {
            _autoComplete = string.IsNullOrWhiteSpace(_query) ? new List<string>() : _engine.AutoComplete(_query).ToList();
            _results = _engine.Search(x => x.Name.Equals(_query)).ToList();
        }

        [ContextMenu("Load")]
        void LoadFromJson() => _data = JsonConvert.DeserializeObject<List<Data>>(_dataString);
        ISearchEngine<Data> _engine;
    }

    [Serializable]
    public class Data
    {
        public string Name;
        public float Price;
        public List<string> Tags;
    }
}
