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
        /// <param name="id"> Index of retrieved item </param>
        /// <returns></returns>
        T Read(int id);

        /// <summary>
        /// Returns all items form repository 
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Updates item in repository 
        /// </summary>
        /// <param name="item"> New item </param>
        void Update(T item);

        /// <summary>
        /// Updates item under given id 
        /// </summary>
        /// <param name="id"> Item id </param>
        /// <param name="item"> New item </param>
        void Update(int id, T item);
    }
}