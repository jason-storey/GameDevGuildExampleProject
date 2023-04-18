namespace JCore.Application.Presenters
{
    public interface StringSearchPresenter
    {
        View View { get; }
        void PerformSearch();
    }
}