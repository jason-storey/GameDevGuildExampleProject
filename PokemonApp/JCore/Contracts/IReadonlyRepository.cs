using System.Collections.Generic;

namespace JCore
{
    public interface IReadonlyRepository<out T> : IEnumerable<T>
    {
        T GetById(string key);
        IEnumerable<T> GetAll();
        T this[string key] { get; }
    }
}