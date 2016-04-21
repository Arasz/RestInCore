using System.Collections.Generic;
using System.Linq;

namespace RESTService.Repository
{
    /// <summary>
    /// Basic repository implementation 
    /// </summary>
    /// <typeparam name="T"> Item type </typeparam>
    public abstract class BaseRepository<T> : IRepository<T>
    {
        protected IList<T> _itemsCollection;

        public BaseRepository()
        {
            _itemsCollection = new List<T>();
        }

        public virtual void Create(T item) => _itemsCollection.Add(item);

        public virtual void Delete(T item) => _itemsCollection.Remove(item);

        public virtual T Read(int id) => _itemsCollection.ElementAt(id);

        public virtual IEnumerable<T> ReadAll() => _itemsCollection.ToList();

        public abstract void Update(T item);

        public abstract void Update(int id, T item);
    }
}