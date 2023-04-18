using System.Collections.Generic;

namespace JCore
{
    public interface IRepository<T> : IReadonlyRepository<T>
    {
        T GetById(string key);
        IEnumerable<T> GetAll();
        void Update(string key, T item);
        void Delete(string key);
        void Create(string key, T item);
    }
}