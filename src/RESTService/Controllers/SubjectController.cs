using Microsoft.AspNet.Mvc;
using RESTService.Links;
using RESTService.Models;
using RESTService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SubjectsRepository _subjectsRepository;

        public SubjectController(IRepository<Subject> subjectsRepository, IServiceProvider serviceProvider)
        {
            _subjectsRepository = subjectsRepository as SubjectsRepository;
            _serviceProvider = serviceProvider;
        }

        [HttpPost("{id}/marks/{studentId}")]
        public async Task<IActionResult> AddMarksForGivenSubject(int id, int studentId, [FromBody] Mark mark)
        {
            //await

            try
            {
                await _subjectsRepository.CreateMarkForSubject(id, mark).ConfigureAwait(false);
                return CreatedAtRoute("GetMarksForSubject", new { controller = "Subject", id, studentId }, mark);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Delete entity with given studentId number 
        /// </summary>
        /// <param name="id"> Unique <paramref name="id"/> number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _subjectsRepository.Read(id).ConfigureAwait(false);

                if (entity == null)
                    return HttpBadRequest($"There is no student with {id} id");

                await _subjectsRepository.Delete(entity).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Delete entities collection 
        /// </summary>
        /// <returns> Information about operation state </returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _subjectsRepository.DeleteAll().ConfigureAwait(false);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Deletes marks for given subject 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="markId"></param>
        /// <returns> Action result </returns>
        [HttpDelete("{id}/marks/{studentId:int?}/{markId:int?}", Name = "DeleteMarksForSubject")]
        public async Task<IActionResult> DeleteMarksForGivenSubject(int id, int studentId, int markId)
        {
            return await ExecuteOperationOnMarks(id, studentId, async (marks, subject) =>
            {
                if (markId > 0)
                {
                    var mark = marks.First(m => m.Id == markId);

                    if (mark == null)
                        return HttpNotFound("Student doesn't have mark of given id");

                    subject.Marks.Remove(mark);
                    await _subjectsRepository.UpdateMarksForSubject(subject.Id, subject.Marks).ConfigureAwait(false);

                    return Ok();
                }

                foreach (var studentMark in marks)
                    subject.Marks.Remove(studentMark);

                return Ok();
            }).ConfigureAwait(false);
        }

        [HttpGet("{id}", Name = "GetMethodSubject")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                    return HttpBadRequest("Wrong id number");

                var entity = await _subjectsRepository.Read(id).ConfigureAwait(false);

                if (entity == null)
                    return HttpNotFound($"Entity with {id} can't be found");

                entity.Resources = new Resources();
                entity.Resources.AddLinks(
                new Link("Parent", $"/api/subject/"),
                new Link("Self", $"/api/subject/{id}"),
                new Link("Next", $"/api/subject/{id + 1}"),
                new Link("Next", $"/api/subject/{id}/marks")
                );
                return Ok(entity);
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get all entities 
        /// </summary>
        /// <returns> Entities collection </returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string teacher)
        {
            if (teacher != null)
            {
                var result = await _subjectsRepository.ReadMatchingStudent(subject => subject.Teacher == teacher);
                return Ok(result);
            }

            var allEntities = await _subjectsRepository.ReadAll();

            if (allEntities != null)
                return Ok(allEntities);

            return HttpNotFound();
        }

        /// <summary>
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="id"> Student id </param>
        /// <param name="studentId"></param>
        /// <param name="markId"></param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{id}/marks/{studentId:int?}/{markId:int?}", Name = "GetMarksForSubject")]
        public async Task<IActionResult> GetMarksForGivenStudent(int id,[FromQuery] int studentId, int markId)
        {
            return await ExecuteOperationOnMarks(id, studentId, async (marks, subject) =>
            {
                if (markId == 0)
                    return Ok(marks);

                var mark = marks.First(m => m.Id == markId);

                mark.Resources.AddLinks(
                    new Link("Parent", $"/api/subject/{id}/marks/{studentId}"),
                    new Link("Self", $"/api/subject/{id}/marks/{studentId}/{markId}"),
                    new Link("Next", $"/api/subject/{id}/marks/{studentId}/{markId + 1}")
                    );

                if (mark == null)
                    return HttpNotFound("Student doesn't have mark of given id");

                return Ok(mark);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates new <paramref name="entity"/> in repository 
        /// </summary>
        /// <param name="entity"> New <paramref name="entity"/> </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Subject entity)
        {
            if (entity == null)
                return HttpBadRequest();

            await _subjectsRepository.Create(entity).ConfigureAwait(false);

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
        public async Task<IActionResult> Put(int id, [FromBody]Subject entity)
        {
            if (entity == null)
                return HttpBadRequest();

            try
            {
                await _subjectsRepository.Update(id, entity).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }

        [HttpPut("{id}/marks/{studentId}/{markId}")]
        public async Task<IActionResult> UpdateMarksForGivenSubject(int id, int studentId, int markId, [FromBody]Mark mark)
        {
            if (mark == null)
                return HttpBadRequest("Wrong mark object format");

            return await ExecuteOperationOnMarks(id, studentId, async (marks, subject) =>
            {
                var firstMark = marks.First(m => m.Id == markId);
                int indexOfMark = subject.Marks.IndexOf(firstMark);

                if (indexOfMark < 0)
                    return HttpNotFound("Student doesn't have any marks with given id");

                mark.Id = firstMark.Id;
                subject.Marks[indexOfMark] = mark;

                await _subjectsRepository.UpdateMarksForSubject(subject.Id, subject.Marks).ConfigureAwait(false);

                return Ok();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="studentId"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private async Task<IActionResult> ExecuteOperationOnMarks(int entityId, int studentId, Func<IEnumerable<Mark>, Subject, Task<IActionResult>> operation = null)
        {
            try
            {
                var subject = await _subjectsRepository.Read(entityId).ConfigureAwait(false);
                if (subject == null)
                    return HttpBadRequest($"Wrong {nameof(Subject)} id");

                if (studentId > 0)
                {
                    var marks = subject.Marks.Where(m => m.StudentId == studentId).ToList();

                    if (!marks.Any())
                        return HttpNotFound($"Student doesn't have any marks");

                    return await operation(marks, subject);
                }

                if (!subject.Marks.Any())
                    return HttpNotFound($"{nameof(Subject)} doesn't have any marks");

                return await operation(subject.Marks, subject);
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }
    }
}