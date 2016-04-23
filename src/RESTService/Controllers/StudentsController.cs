using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class StudentsController : BaseController<Student>
    {
        public StudentsController(IRepository<Entity> entitiesRepository) : base(entitiesRepository)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="markId"></param>
        /// <returns></returns>
        [HttpDelete("{studentId}/marks/{markId:int?}")]
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
    }
}