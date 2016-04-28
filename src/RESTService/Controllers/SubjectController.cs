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
    public class SubjectController : BaseController<Subject>
    {
        public SubjectController(IRepository<Entity> entitiesRepository) : base(entitiesRepository)
        {
        }

        [HttpPost("{id}/marks/{studentId}")]
        public IActionResult AddMarksForGivenSubject(int id, int studentId, [FromBody] Mark mark)
        {
            var subject = _entitiesRepository.Read<Subject>(id);

            if (subject == null)
                return HttpBadRequest($"Wrong {ControllerName} id");

            subject.Marks.Add(mark);

            return CreatedAtRoute("GetMarksForSubject", new { controller = "Subject", id, studentId }, mark);
        }

        /// <summary>
        /// Deletes marks for given subject 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="markId"></param>
        /// <returns> Action result </returns>
        [HttpDelete("{id}/marks/{studentId:int?}/{markId:int?}", Name = "DeleteMarksForSubject")]
        public IActionResult DeleteMarksForGivenSubject(int id, int studentId, int markId)
        {
            return ExecuteOperationOnMarks(id, studentId, (marks, subject) =>
            {
                if (markId > 0)
                {
                    var mark = marks.First(m => m.Id == markId);

                    if (mark == null)
                        return HttpNotFound("Student doesn't have mark of given id");

                    subject.Marks.Remove(mark);

                    return Ok();
                }

                foreach (var studentMark in marks)
                    subject.Marks.Remove(studentMark);

                return Ok();
            });
        }

        [HttpGet("{id}", Name = "GetMethodSubject")]
        public override IActionResult Get(int id) => base.Get(id);

        /// <summary>
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="id"> Student id </param>
        /// <param name="studentId"></param>
        /// <param name="markId"></param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{id}/marks/{studentId:int?}/{markId:int?}", Name = "GetMarksForSubject")]
        public IActionResult GetMarksForGivenStudent(int id, int studentId, int markId)
        {
            return ExecuteOperationOnMarks(id, studentId, (marks, subject) =>
            {
                if (markId == 0)
                    return Ok(marks);

                var mark = marks.First(m => m.Id == markId);

                if (mark == null)
                    return HttpNotFound("Student doesn't have mark of given id");

                return Ok(mark);
            });
        }

        [HttpPut("{id}/marks/{studentId}/{markId}")]
        public IActionResult UpdateMarksForGivenSubject(int id, int studentId, int markId, [FromBody]Mark mark)
        {
            if (mark == null)
                return HttpBadRequest("Wrong mark object format");

            return ExecuteOperationOnMarks(id, studentId, (marks, subject) =>
            {
                var firstMark = marks.First(m => m.Id == markId);
                int indexOfMark = subject.Marks.IndexOf(firstMark);

                if (indexOfMark < 0)
                    return HttpNotFound("Student doesn't have any marks with given id");

                mark.Id = firstMark.Id;
                subject.Marks[indexOfMark] = mark;

                return Ok();
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="studentId"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private IActionResult ExecuteOperationOnMarks(int entityId, int studentId, Func<IEnumerable<Mark>, Subject, IActionResult> operation = null)
        {
            try
            {
                var subject = _entitiesRepository.Read<Subject>(entityId);
                if (subject == null)
                    return HttpBadRequest($"Wrong {ControllerName} id");

                if (studentId > 0)
                {
                    var marks = subject.Marks.Where(m => m.StudentId == studentId).ToList();

                    if (!marks.Any())
                        return HttpNotFound($"Student doesn't have any marks");

                    return operation(marks, subject);
                }

                if (!subject.Marks.Any())
                    return HttpNotFound($"{ControllerName} doesn't have any marks");

                return operation(subject.Marks, subject);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }
    }
}