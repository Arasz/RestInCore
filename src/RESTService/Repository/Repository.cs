﻿using RESTService.Models;
using RESTService.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RESTService.Utils;

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
    private readonly Dictionary<int, Entity> _entities;


    public Repository(UniqueIdentityProvider identityProvider)
    {

        IEnumerable<Entity> initializationList = new DataInitializer(identityProvider).Data;
        
        foreach (var entity in initializationList)
        {
            _entities[entity.Id] = entity;
        }

    }

    public void Create(Entity item)
    {
        var studentId = item.Id;

        if (_entities.ContainsKey(studentId))
            return;

        _entities[item.Id] = item;
    }

    /// <exception cref="KeyNotFoundException"> Student with given id don't exist </exception>
    public void Delete(Entity item)
    {
        if (!_entities.ContainsKey(item.Id))
            throw new KeyNotFoundException("Student with given id don't exist");
        _entities.Remove(item.Id);
    }

    /// <exception cref="KeyNotFoundException"> Condition. </exception>
    public Entity Read(int id)
    {
        if (!_entities.ContainsKey(id))
            throw new KeyNotFoundException("Student with given id don't exist");

        return _entities[id];
    }

    public IEnumerable<Entity> ReadAll() => _entities.Values;

    /// <exception cref="ArgumentException"> "Can't find entity in repository </exception>
    public void Update(Entity item)
    {
        Update(item.Id, item);
    }

    public void Update(int id, Entity item)
    {
        if (!_entities.ContainsKey(id))
            throw new ArgumentException("Can't find entity in repository");

        if (id != item.Id)
            throw new ArgumentException("Given id different from student id");

        _entities[id] = item;
    }
}
}