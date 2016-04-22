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
    public class StudentsController : Controller
    {
        /// <summary>
        /// Application data access abstract layer 
        /// </summary>
        private readonly IRepository<Entity> _entitiesRepository;

        public StudentsController(IRepository<Entity> entitiesRepository)
        {
            _entitiesRepository = entitiesRepository;
        }

        /// <summary>
        /// Delete student with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var student = _entitiesRepository.Read(id);
                _entitiesRepository.Delete(student);

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get student with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Student with given id number </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var student = _entitiesRepository.Read(id);
                return Json(student);
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
        [HttpGet]
        public IActionResult GetAll()
        {
            var allStudents = _entitiesRepository.ReadAll<Student>();

            if (allStudents != null)
                return Json(allStudents);

            return HttpNotFound();
        }

        /// <summary>
        /// Creates new student in repository 
        /// </summary>
        /// <param name="student"> New student </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Student student)
        {
            if (student == null)
                return HttpBadRequest();

            _entitiesRepository.Create(student);
            return Ok();
        }

        /// <summary>
        /// Updates student under given id 
        /// </summary>
        /// <param name="id"> Update student id </param>
        /// <param name="student"> New student state </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Student student)
        {
            if (student == null)
                return HttpBadRequest();

            try
            {
                _entitiesRepository.Update(id, student);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}