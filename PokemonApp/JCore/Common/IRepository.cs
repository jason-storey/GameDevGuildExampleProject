using System.Collections.Generic;

namespace JCore.Common
{
    public interface IRepository<T>
    {
        T GetById(string key);
        IEnumerable<T> GetAll();
        void Update(string key, T item);
        void Delete(string key);
        void Create(string key, T item);
    }
}