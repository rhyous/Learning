using System.Collections.Generic;
using System.Linq;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        T Create(T entity);
        IEnumerable<T> Create(IEnumerable<T> entities);
        IQueryable<T> Read();
        T Read(int id);
        T Update(T entity);
        bool Delete(T entity);
    }
}
