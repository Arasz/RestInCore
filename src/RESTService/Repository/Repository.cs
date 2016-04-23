﻿using RESTService.Models;
using RESTService.Providers;
using RESTService.Utils;
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

        public int Create(Entity entity)
        {
            if (_entities.ContainsKey(entity.Id))
                return entity.Id;

            if (entity.Id == 0)
                entity.Id = _identityProvider.Id;

            _entities[entity.Id] = entity;

            return entity.Id;
        }

        /// <exception cref="KeyNotFoundException">
        /// <see cref="Entity"/> with given id don't exist
        /// </exception>
        public void Delete(Entity entity)
        {
            if (!_entities.ContainsKey(entity.Id))
                throw new KeyNotFoundException("Entity with given id don't exist");
            _entities.Remove(entity.Id);
        }

        /// <exception cref="ArgumentNullException"> <paramref name="key"/> is null. </exception>
        public void DeleteAll<E>() where E : Entity
        {
            foreach (var entity in _entities.Where(entity => entity.Value is E).ToList())
            {
                _entities.Remove(entity.Key);
            }
        }

        public E Read<E>(int id) where E : Entity
        {
            return Read(id) as E;
        }

        /// <exception cref="KeyNotFoundException">
        /// <see cref="Entity"/> with given id don't exist.
        /// </exception>
        public Entity Read(int id)
        {
            if (!_entities.ContainsKey(id))
                throw new KeyNotFoundException("Entity with given id don't exist");

            return _entities[id];
        }

        public IEnumerable<T> ReadAll<T>() where T : Entity
        {
            return _entities.Values.Where(entity => entity.GetType() == typeof(T)).Cast<T>().ToList();
        }

        public void Update(Entity entity)
        {
            Update(entity.Id, entity);
        }

        /// <exception cref="KeyNotFoundException">
        /// Can't find <paramref name="entity"/> in repository
        /// </exception>
        public void Update(int id, Entity entity)
        {
            if (!_entities.ContainsKey(id))
                throw new KeyNotFoundException("Can't find entity in repository");

            if (entity.Id == 0)
                entity.Id = id;

            _entities[id] = entity;
        }
    }
}