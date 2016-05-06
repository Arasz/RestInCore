using RESTService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTService.Repository
{
    public interface IRepository<T>
        where T : Entity
    {
        Task Create(T entity);

        Task Delete(T entity);

        Task DeleteAll();

        Task<T> Read(int id);

        Task<IEnumerable<T>> ReadAll();

        Task Update(T entity);

        Task Update(int id, T entity);
    }
}