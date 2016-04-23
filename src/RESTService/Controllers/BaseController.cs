using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System;
using System.Collections.Generic;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller with basic operations 
    /// </summary>
    public abstract class BaseController<T> : Controller
        where T : Entity
    {
        /// <summary>
        /// Application data access abstract layer 
        /// </summary>
        protected readonly IRepository<Entity> _entitiesRepository;

        /// <summary>
        /// <see cref="Controller"/> name taken form generic type <typeparamref name="T"/> 
        /// </summary>
        protected virtual string ControllerName { get; } = typeof(T).Name;

        public BaseController(IRepository<Entity> entitiesRepository)
        {
            _entitiesRepository = entitiesRepository;
        }

        /// <summary>
        /// Delete entity with given studentId number 
        /// </summary>
        /// <param name="id"> Unique <paramref name="id"/> number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
        {
            try
            {
                var entity = _entitiesRepository.Read(id);

                if (!(entity is T))
                    return HttpBadRequest($"There is no {nameof(T)} with given id");

                _entitiesRepository.Delete(entity);

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Delete entities collection 
        /// </summary>
        /// <returns> Information about operation state </returns>
        [HttpDelete]
        public virtual IActionResult Delete()
        {
            try
            {
                _entitiesRepository.DeleteAll<T>();

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get entity with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Entity with given id number </returns>
        public virtual IActionResult Get(int id)
        {
            try
            {
                if (id == 0)
                    return HttpBadRequest("Wrong id number");

                var entity = _entitiesRepository.Read(id);

                if (entity is T)
                    return Ok(entity);

                return HttpBadRequest($"There is no {nameof(T)} with given id");
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get all entities 
        /// </summary>
        /// <returns> Entities collection </returns>
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            var allEntities = _entitiesRepository.ReadAll<T>();

            if (allEntities != null)
                return Ok(allEntities);

            return HttpNotFound();
        }

        /// <summary>
        /// Creates new <paramref name="entity"/> in repository 
        /// </summary>
        /// <param name="entity"> New <paramref name="entity"/> </param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Post([FromBody]T entity)
        {
            if (entity == null)
                return HttpBadRequest();

            _entitiesRepository.Create(entity);
            var controllerName = GetType().Name.Replace("Controller", "");
            return CreatedAtRoute("GetMethod" + controllerName, new { controller = controllerName, id = entity.Id }, entity);
        }

        /// <summary>
        /// Updates entity under given id 
        /// </summary>
        /// <param name="id"> Update entity id </param>
        /// <param name="entity"> New entity state </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual IActionResult Put(int id, [FromBody]T entity)
        {
            if (entity == null)
                return HttpBadRequest();

            try
            {
                _entitiesRepository.Update(id, entity);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}