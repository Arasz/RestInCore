using RESTService.Models;
using System.Collections.Generic;

namespace RESTService.Repository
{
    /// <summary>
    /// Base repository interface with all basics operations 
    /// </summary>
    /// <typeparam name="T"> Type of entities which repository is holding </typeparam>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Adds new entity to repository 
        /// </summary>
        /// <param name="entity"></param>
        int Create(T entity);

        /// <summary>
        /// Removes entity from repository 
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Deletes all entities of given type 
        /// </summary>
        /// <typeparam name="E"></typeparam>
        void DeleteAll<E>() where E : T;

        /// <summary>
        /// Returns entity of given type under given index 
        /// </summary>
        /// <param name="id"> Index of retrieved entity </param>
        /// <typeparam name="E"> Retrieved entity type </typeparam>
        /// <returns> If exist entity otherwise <see langword="null"/> </returns>
        E Read<E>(int id) where E : Entity;

        /// <summary>
        /// Returns entity under given index 
        /// </summary>
        /// <param name="id"> Index of retrieved entity </param>
        /// <returns></returns>
        T Read(int id);

        /// <summary>
        /// Returns all entities form repository 
        /// </summary>
        /// <returns></returns>
        IEnumerable<E> ReadAll<E>() where E : T;

        /// <summary>
        /// Updates entity in repository 
        /// </summary>
        /// <param name="entity"> New entity </param>
        void Update(T entity);

        /// <summary>
        /// Updates entity under given id 
        /// </summary>
        /// <param name="id"> Item id </param>
        /// <param name="entity"> New entity </param>
        void Update(int id, T entity);
    }
}