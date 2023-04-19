using JCore;
using JCore.AdvancedSearch;
using JCore.Application;
using JCore.Collections;
using JCore.Search;
using JSearch;

namespace PokemonApp.SimpleSearchApp
{
    public class SearchApplicationFactory<T>
    {
        ISearchEngine<T> CreateSearchEngine() => new SearchEngineFactory<T>(new InvertedIndex<string>()).Create(_data);

        #region plumbing

        public SearchApplicationFactory(ViewFactory<T> views,IReadonlyRepository<T> data,
            PropertySelectors<T> properties)
        {
            _views = views;
            _data = data;
            _properties = properties;
        }
        public App<T> CreateApplication() => new App<T>(_views, CreateSearchEngine());
        readonly ViewFactory<T> _views;
        readonly IReadonlyRepository<T> _data;
        readonly PropertySelectors<T> _properties;

        #endregion
    }
}