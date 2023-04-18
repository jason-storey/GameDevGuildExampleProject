using System.Collections.Generic;

namespace JCore
{
    public interface IRepository<T> : IReadonlyRepository<T>
    {
        void Update(string key, T item);
        void Delete(string key);
        void Add(T item);
    }
}