using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System;
using System.Collections.Generic;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        /// <summary>
        /// Application data access abstract layer 
        /// </summary>
        private readonly IRepository<Entity> _entitiesRepository;

        public SubjectsController(IRepository<Entity> entitiesRepository)
        {
            _entitiesRepository = entitiesRepository;
        }

        /// <summary>
        /// Delete subject with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var subject = _entitiesRepository.Read(id);
                _entitiesRepository.Delete(subject);

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get subject with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Student with given id number </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var subject = _entitiesRepository.Read(id);
                return Json(subject);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get all students 
        /// </summary>
        /// <returns> Students collection </returns>
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var allSubjects = _entitiesRepository.ReadAll<Subject>();

            if (allSubjects != null)
                return Json(allSubjects);

            return HttpNotFound();
        }

        /// <summary>
        /// Creates new subject in repository 
        /// </summary>
        /// <param name="subject"> New subject </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Subject subject)
        {
            if (subject == null)
                return HttpBadRequest();

            _entitiesRepository.Create(subject);
            return Ok();
        }

        /// <summary>
        /// Updates subject under given id 
        /// </summary>
        /// <param name="id"> Update subject id </param>
        /// <param name="subject"> New subject state </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Subject subject)
        {
            if (subject == null)
                return HttpBadRequest();

            try
            {
                _entitiesRepository.Update(id, subject);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}