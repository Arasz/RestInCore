using Microsoft.AspNet.Mvc;
using RESTService.Links;
using RESTService.Models;
using RESTService.Repository;
using RESTService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    /// <summary>
    /// Controller responsible for students models 
    /// </summary>
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StudentsRepository _studentsRepository;

        public StudentsController(IRepository<Student> studentsRepository, IServiceProvider serviceProvider)
        {
            _studentsRepository = studentsRepository as StudentsRepository;
            _serviceProvider = serviceProvider;
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
                var entity = await _studentsRepository.Read(id).ConfigureAwait(false);

                if (entity == null)
                    return HttpBadRequest($"There is no student with {id} id");

                await _studentsRepository.Delete(entity).ConfigureAwait(false);

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
                await _studentsRepository.DeleteAll().ConfigureAwait(false);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get entity with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Entity with given id number </returns>
        [HttpGet("{id}", Name = "GetMethodStudents")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                    return HttpBadRequest("Wrong id number");

                var entity = await _studentsRepository.Read(id).ConfigureAwait(false);

                if (entity == null)
                    return HttpNotFound($"Entity with {id} can't be found");

                //HACK: Obejście problemu
                entity.Resources = new Resources();
                entity.Resources.AddLinks(
                    new Link("Parent", $"/api/student/"),
                    new Link("Self", $"/api/student/{id}/"),
                    new Link("Next", $"/api/student/{id + 1}/"),
                    new Link("Grades", $"/api/student/{id}/marks/")
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
        public async Task<IActionResult> GetAll([FromQuery]string name, [FromQuery]string surname,
            [FromQuery]DateTime afterDate, [FromQuery]DateTime beforeDate)
        {
            if (name != null && surname != null)
            {
                var students = await _studentsRepository.ReadMatchingStudent(student => student.Surname == surname && student.Name == name);
                return Ok(students);
            }

            if (name != null)
            {
                var students = await _studentsRepository.ReadMatchingStudent(student => student.Name == name);
                return Ok(students);
            }

            if (surname != null)
            {
                var students = await _studentsRepository.ReadMatchingStudent(student => student.Surname == surname);
                return Ok(students);
            }

            if (afterDate != default(DateTime) && beforeDate != default(DateTime))
            {
                var students = await _studentsRepository.ReadMatchingStudent(student =>
                student.Birthday.CompareTo(afterDate) > 0 && student.Birthday.CompareTo(beforeDate) < 0);
                return Ok(students);
            }

            if (afterDate != default(DateTime))
            {
                var students = await _studentsRepository.ReadMatchingStudent(student => student.Birthday.CompareTo(afterDate) > 0);
                return Ok(students);
            }

            if (beforeDate != default(DateTime))
            {
                var students = await _studentsRepository.ReadMatchingStudent(student => student.Birthday.CompareTo(beforeDate) < 0);
                return Ok(students);
            }

            var initializer = _serviceProvider.GetService(typeof(DataInitializer)) as DataInitializer;

            await initializer.PopulateBase(false).ConfigureAwait(false);

            var allEntities = await _studentsRepository.ReadAll().ConfigureAwait(false);

            if (allEntities != null)
                return Ok(allEntities);

            return HttpNotFound();
        }

        /// <summary>
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="id"> Student id </param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{id}/marks/{markId:int?}")]
        public async Task<IActionResult> GetMarksForGivenStudent(int id, int markId, [FromQuery]double? greaterThan, [FromQuery]double? lessThan)
        {
            try
            {
                var subjectsRepository = _serviceProvider.GetService(typeof(IRepository<Subject>)) as IRepository<Subject>;
                var subjects = await subjectsRepository.ReadAll().ConfigureAwait(false);

                var marks = subjects.Aggregate(new List<Mark>(),
                        (list, subject) =>
                        {
                            list.AddRange(subject.Marks.Where(m => m.StudentId == id));
                            return list;
                        });

                if (!marks.Any())
                    return HttpNotFound("Student doesn't have any marks");

                if (markId == 0)
                {
                    if (greaterThan != null && lessThan != null)
                    {
                        var result = marks.Where(mark1 => mark1.Value > greaterThan.Value && mark1.Value < lessThan.Value);
                        return Ok(result.ToList());
                    }
                    if (greaterThan != null)
                    {
                        var result = marks.Where(mark1 => mark1.Value > greaterThan.Value);
                        return Ok(result.ToList());
                    }
                    if (lessThan != null)
                    {
                        var result = marks.Where(mark1 => mark1.Value < lessThan.Value);
                        return Ok(result.ToList());
                    }
                    return Ok(marks);
                }

                var mark = marks.First(m => m.Id == markId);

                mark.Resources.AddLinks(
                               new Link("Parent", $"api/student/{id}/marks"),
                               new Link("Self", $"api/student/{id}/marks/{markId}"),
                               new Link("Next", $"api/student/{id}/marks/{++markId}")
                               );

                if (mark == null)
                    return HttpNotFound("Student doesn't have mark of given id");

                return Ok(mark);
            }
            catch (Exception exception)
            {
                return HttpNotFound(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Student entity)
        {
            if (entity == null)
                return HttpBadRequest();

            await _studentsRepository.Create(entity).ConfigureAwait(false);

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
        public async Task<IActionResult> Put(int id, [FromBody]Student entity)
        {
            if (entity == null)
                return HttpBadRequest();

            try
            {
                await _studentsRepository.Update(id, entity).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}