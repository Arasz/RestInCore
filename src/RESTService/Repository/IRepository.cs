using System.Collections.Generic;

namespace RESTService.Repository
{
    public interface IRepository<T>
    {
        void Create(T newItem);

        void Delete(T item);

        T Retrive(int index);

        IEnumerable<T> RetriveAll();

        void Update(int index, T newItem);
    }
}