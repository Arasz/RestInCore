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

        [HttpPost("{id}/marks")]
        public IActionResult AddMarksForGivenSubject(int id, [FromBody] StudentMark mark)
        {
            var subject = _entitiesRepository.Read<Subject>(id);

            if (subject == null)
                return HttpBadRequest($"Wrong {ControllerName} id");

            var newId = _entitiesRepository.Create(mark);
            subject.Marks.Add(_entitiesRepository.Read<StudentMark>(newId));

            return CreatedAtRoute("GetMarksForSubject", new { controller = "Subject", id, markId = newId }, mark);
        }

        /// <summary>
        /// Deletes marks for given subject 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="markId"></param>
        /// <returns> Action result </returns>
        [HttpDelete("{id}/marks/{markId:int?}", Name = "DeleteMarksForSubject")]
        public IActionResult DeleteMarksForGivenSubject(int id, int markId)
        {
            return ExecuteOperationOnMarks(id, markId, (marks, subject) =>
            {
                foreach (var studentMark in marks)
                {
                    subject.Marks.Remove(studentMark);
                    _entitiesRepository.Delete(studentMark);
                }
                return Ok();
            });
        }

        [HttpGet("{id}", Name = "GetMethodSubject")]
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
        [HttpGet("{id}/marks/{markId:int?}", Name = "GetMarksForSubject")]
        public IActionResult GetMarksForGivenStudent(int id, int markId)
        {
            return ExecuteOperationOnMarks(id, markId, (marks, subject) => marks.Count() > 1 ? Ok(marks) : Ok(marks.First()));
        }

        [HttpPut("{id}/marks/{markId}")]
        public IActionResult UpdateMarksForGivenSubject(int id, int markId, [FromBody]StudentMark mark)
        {
            return ExecuteOperationOnMarks(id, markId, (marks, subject) =>
            {
                int indexOfMark = subject.Marks.IndexOf(marks.First());

                _entitiesRepository.Update(markId, mark);

                subject.Marks[indexOfMark] = _entitiesRepository.Read<StudentMark>(markId);

                return Ok();
            });
        }

        private IActionResult ExecuteOperationOnMarks(int entityId, int markId, Func<IEnumerable<StudentMark>, Subject, IActionResult> operation = null)
        {
            try
            {
                var subject = _entitiesRepository.Read<Subject>(entityId);
                if (subject == null)
                    return HttpBadRequest($"Wrong {ControllerName} id");

                if (markId > 0)
                {
                    var mark = _entitiesRepository.Read<StudentMark>(markId);

                    if (mark == null || !subject.Marks.Contains(mark))
                        return HttpBadRequest("Wrong mark id");

                    return operation(new List<StudentMark> { mark }, subject);
                }

                var marks = subject.Marks;

                if (!marks.Any())
                    return HttpNotFound($"{ControllerName} doesn't have any marks");

                return operation(marks, subject);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }
    }
}