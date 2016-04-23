using RESTService.Models;
using RESTService.Providers;
using RESTService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RESTService.Repository
{
    /// <summary>
    /// Repository containing Students 
    /// </summary>
    [DataContract]
    public class Repository : IRepository<Entity>
    {
        /// <summary>
        /// All students collection 
        /// </summary>
        [DataMember]
        private readonly Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();

        private readonly IIdentityProvider<int> _identityProvider;

        public Repository(IIdentityProvider<int> identityProvider)
        {
            _identityProvider = identityProvider;

            IEnumerable<Entity> initializationList = new DataInitializer(identityProvider).Data;

            foreach (var entity in initializationList)
            {
                _entities[entity.Id] = entity;
            }
        }

        public void Create(Entity entity)
        {
            if (_entities.ContainsKey(entity.Id))
                return;

            if (entity.Id == 0)
                entity.Id = _identityProvider.Id;

            _entities[entity.Id] = entity;
        }

        /// <exception cref="KeyNotFoundException"> Student with given id don't exist </exception>
        public void Delete(Entity entity)
        {
            if (!_entities.ContainsKey(entity.Id))
                throw new KeyNotFoundException("Student with given id don't exist");
            _entities.Remove(entity.Id);
        }

        /// <exception cref="ArgumentNullException"> <paramref name="key"/> is null. </exception>
        public void DeleteAll<E>() where E : Entity
        {
            foreach (var entity in _entities.Where(entity => entity.Value is E))
            {
                _entities.Remove(entity.Key);
            }
        }

        /// <exception cref="KeyNotFoundException"> Condition. </exception>
        public Entity Read(int id)
        {
            if (!_entities.ContainsKey(id))
                throw new KeyNotFoundException("Student with given id don't exist");

            return _entities[id];
        }

        public IEnumerable<T> ReadAll<T>() where T : Entity
        {
            return _entities.Values.Where(entity => entity.GetType() == typeof(T)).Cast<T>().ToList();
        }

        /// <exception cref="ArgumentException"> "Can't find entity in repository </exception>
        public void Update(Entity entity)
        {
            Update(entity.Id, entity);
        }

        public void Update(int id, Entity entity)
        {
            if (!_entities.ContainsKey(id))
                throw new ArgumentException("Can't find entity in repository");

            if (id != entity.Id)
                throw new ArgumentException("Given id different from student id");

            _entities[id] = entity;
        }
    }
}