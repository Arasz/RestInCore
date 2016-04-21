using System.Collections.Generic;
using System.Linq;

namespace RESTService.Repository
{
    /// <summary>
    /// Basic repository implementation 
    /// </summary>
    /// <typeparam name="T"> Item type </typeparam>
    public class BaseRepository<T> : IRepository<T>
    {
        protected IList<T> _itemsCollection;

        public BaseRepository()
        {
            _itemsCollection = new List<T>();
        }

        public virtual void Create(T item) => _itemsCollection.Add(item);

        public virtual void Delete(T item) => _itemsCollection.Remove(item);

        public virtual T Retrieve(int index) => _itemsCollection[index];

        public virtual IEnumerable<T> RetrieveAll() => _itemsCollection.ToList();

        public virtual void Update(int index, T item) => _itemsCollection[index] = item;
    }
}