using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Delete student with given studentId number 
        /// </summary>
        /// <param name="id"> Unique studentId number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var student = _entitiesRepository.Read(id);

                if (!(student is Student))
                    return HttpBadRequest("There is no student with given id");

                _entitiesRepository.Delete(student);

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Delete students collection 
        /// </summary>
        /// <returns> Information about operation state </returns>
        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                _entitiesRepository.DeleteAll<Student>();

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="markId"></param>
        /// <returns></returns>
        [HttpDelete("{studentId}/marks/{markID:int?}")]
        public IActionResult DeleteMarksForGivenStudent(int studentId, int markId)
        {
            try
            {
                if (markId > 0)
                {
                    var studentMark = _entitiesRepository.Read(markId) as StudentMark;

                    if (studentMark == null || studentId != studentMark.StudentId)
                        return HttpBadRequest("Wrong mark id");

                    _entitiesRepository.Delete(studentMark);

                    return Ok();
                }

                var marks = _entitiesRepository.ReadAll<StudentMark>().Where(mark => mark.StudentId == studentId).ToList();

                if (!marks.Any())
                    return HttpNotFound("Student doesn't have any marks");

                foreach (var studentMark in marks)
                {
                    _entitiesRepository.Delete(studentMark);
                }

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get student with given studentId number 
        /// </summary>
        /// <param name="id"> Unique studentId number </param>
        /// <returns> Student with given studentId number </returns>
        [HttpGet("{id}", Name = "GetStudent")]
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
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="studentId"> Student id </param>
        /// <param name="markId"> MArk id (optional) </param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{studentId}/marks/{markId:int?}")]
        public IActionResult GetMarksForGivenStudent(int studentId, int markId)
        {
            try
            {
                if (markId > 0)
                {
                    var studentMark = _entitiesRepository.Read(markId) as StudentMark;

                    if (studentMark == null || studentId != studentMark.StudentId)
                        return HttpBadRequest("Wrong mark id");

                    return Ok(studentMark);
                }

                var marks = _entitiesRepository.ReadAll<StudentMark>().Where(mark => mark.StudentId == studentId).ToList();

                if (!marks.Any())
                    return HttpNotFound("Student doesn't have any marks");

                return Ok(marks);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
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
            return CreatedAtRoute("GetStudent", new { controller = "Students", id = student.Id }, student);
        }

        /// <summary>
        /// Updates student under given studentId 
        /// </summary>
        /// <param name="id"> Update student studentId </param>
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