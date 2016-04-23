using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RESTService.Controllers
{
    /// <summary>
    /// Controller responsible for students models 
    /// </summary>
    [Route("api/[controller]")]
    public class StudentController : BaseController<Student>
    {
        public StudentController(IRepository<Entity> entitiesRepository) : base(entitiesRepository)
        {
        }

        [HttpGet("{id}", Name = "GetMethodStudent")]
        public override IActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="id"> Student id </param>
        /// <param name="markId"> MArk id (optional) </param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{id}/marks/{markId:int?}")]
        public IActionResult GetMarksForGivenStudent(int id, int markId)
        {
            try
            {
                if (markId > 0)
                {
                    var studentMark = _entitiesRepository.Read(markId) as StudentMark;

                    if (studentMark == null || id != studentMark.StudentId)
                        return HttpBadRequest("Wrong mark id");

                    return Ok(studentMark);
                }

                var marks = _entitiesRepository.ReadAll<StudentMark>().Where(mark => mark.StudentId == id).ToList();

                if (!marks.Any())
                    return HttpNotFound("Student doesn't have any marks");

                return Ok(marks);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }
    }
}