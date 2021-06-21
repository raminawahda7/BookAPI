using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories.Interfaces
{
    public interface IRepository<T1, T2> where T1 : class
    {
        Task<IEnumerable<T1>> Get();
        Task<T1> Get(T2 id);
        Task<T1> Create(T1 entity);
        Task<T1> Update(T1 entity);
        Task <Exception> Delete(T2 id);
    }
}
