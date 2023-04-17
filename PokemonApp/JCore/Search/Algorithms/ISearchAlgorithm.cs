namespace JCore.Search
{
    public interface ISearchAlgorithm
    {
        bool IsMatch(string searchTerm, string content);
    }
}