using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTService.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private IRepository<Subject> _subjects;

        public SubjectsController(IRepository<Subject> subjects)
        {
            _subjects = subjects;
        }

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
                var student = _subjects.Read(studentId);
                _subjects.Delete(student);

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
                var student = _subjects.Read(studentId);
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
            var allStudents = _subjects.ReadAll();

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

            _subjects.Create(student);
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
                _subjects.Update(studentId, student);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}