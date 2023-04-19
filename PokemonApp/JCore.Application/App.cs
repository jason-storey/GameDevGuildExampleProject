using JCore.AdvancedSearch;
using JCore.Application.UseCases;

namespace JCore.Application
{
    public class App<T>
    {
        readonly UsecaseFactory<T> _usecases;

        public App(ViewFactory<T> uis, ISearchEngine<T> searchEngine) =>
            _usecases = new UsecaseFactory<T>(searchEngine,new QueryFactory<T>(searchEngine), uis);

        public SearchByString<T> Search() => _usecases.Search();
    }
}