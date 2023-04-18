using JCore.Application.UseCases;

namespace JCore.Application
{
    public class App<T>
    {
        UsecaseFactory<T> _usecases;

        public App(ViewFactory<T> uis, IReadonlyRepository<T> repos) =>
            _usecases = new UsecaseFactory<T>(new QueryFactory<T>(repos), uis);

        public SearchByString<T> Search() => _usecases.Search();
    }
}