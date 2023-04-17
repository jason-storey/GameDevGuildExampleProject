using System.Collections.Generic;

namespace JCore
{
    public interface IListRepository<T>
    {
        IEnumerable<T> GetValuesById(string key);
        int GetElementCount(string key);
        bool AddElement(string key, T item);
        bool RemoveElement(string key, T item);
        bool Update(string key, T item);
        bool Delete(string key, T item);
        bool DeleteAll(string key);
        void SetElements(string term, IEnumerable<T> items);
    }
}