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
        private readonly IRepository<Student> _studentsRepository;

        public StudentsController(IRepository<Student> studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        // DELETE api/values/5
        /// <summary>
        /// Delete student with given id number 
        /// </summary>
        /// <param name="studentId"> Unique id number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{studentId}")]
        public IActionResult Delete(int studentId)
        {
            try
            {
                var student = _studentsRepository.Read(studentId);
                _studentsRepository.Delete(student);

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
        /// <param name="studentId"> Unique id number </param>
        /// <returns> Student with given id number </returns>
        [HttpGet("{studentId}")]
        public IActionResult Get(int studentId)
        {
            try
            {
                var student = _studentsRepository.Read(studentId);
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
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var allStudents = _studentsRepository.ReadAll();

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

            _studentsRepository.Create(student);
            return Ok();
        }

        /// <summary>
        /// Updates student under given id 
        /// </summary>
        /// <param name="studentId"> Update student id </param>
        /// <param name="student"> New student state </param>
        /// <returns></returns>
        [HttpPut("{studentId}")]
        public IActionResult Put(int studentId, [FromBody]Student student)
        {
            if (student == null)
                return HttpBadRequest();

            try
            {
                _studentsRepository.Update(studentId, student);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}