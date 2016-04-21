using System.Collections.Generic;
using System.Linq;

namespace RESTService.Repository
{
    public class BaseRepository<T> : IRepository<T>
    {
        protected IList<T> _itemsCollection;

        public virtual void Create(T item) => _itemsCollection.Add(item);

        public virtual void Delete(T item) => _itemsCollection.Remove(item);

        public virtual T Retrieve(int index) => _itemsCollection.ElementAt(index);

        public virtual IEnumerable<T> RetrieveAll() => _itemsCollection.ToList();

        public virtual void Update(int index, T item) => _itemsCollection[index] = item;
    }
}