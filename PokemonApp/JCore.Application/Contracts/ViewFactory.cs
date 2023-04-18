using JCore.Application.Views;

namespace JCore.Application
{
    public interface ViewFactory<in T>
    {
        StringSearchView<T> Searching();
    }
}