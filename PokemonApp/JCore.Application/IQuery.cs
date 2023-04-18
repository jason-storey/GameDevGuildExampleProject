using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCore.Application
{
    public interface IQuery<T>
    {
        Task<IEnumerable<T>> Execute();
    }
}