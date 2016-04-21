using System.Collections.Generic;

namespace RESTService.Repository
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds new item to repository 
        /// </summary>
        /// <param name="item"></param>
        void Create(T item);

        /// <summary>
        /// Removes item from repository 
        /// </summary>
        /// <param name="item"></param>
        void Delete(T item);

        /// <summary>
        /// Returns item under given index 
        /// </summary>
        /// <param name="index"> Index of retrieved item </param>
        /// <returns></returns>
        T Retrieve(int index);

        /// <summary>
        /// Returns all items form repository 
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> RetrieveAll();

        /// <summary>
        /// Updates item in repository 
        /// </summary>
        /// <param name="index"> Updated item <paramref name="index"/> </param>
        /// <param name="item"> New item </param>
        void Update(int index, T item);
    }
}