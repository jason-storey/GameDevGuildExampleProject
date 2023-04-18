using JCore;
using JCore.Application;
using JCore.Search;

namespace PokemonApp.SimpleSearchApp
{
    public class SearchApplicationFactory<T>
    {
        ISearchEngine<T> CreateSearchEngine() => 
            new SearchEngineV1<T>(
                _data, 
                new Autocomplete<T>(_properties),
                new BKLevensteinSpellingCorrector(), _properties);

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