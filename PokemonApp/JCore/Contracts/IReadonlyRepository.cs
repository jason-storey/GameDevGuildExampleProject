using System.Collections.Generic;

namespace JCore
{
    public interface IReadonlyRepository<out T>
    {
        T GetById(string key);
        IEnumerable<T> GetAll();
    }
}