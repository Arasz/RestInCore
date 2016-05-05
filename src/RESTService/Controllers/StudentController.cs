using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
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
    public class StudentController : BaseController<Student>
    {
        private int _id;
        private StudentsRepository _repo;

        public StudentController(IRepository<Entity> entitiesRepository, StudentsRepository repository) : base(entitiesRepository)
        {
            _repo = repository;
            var student = new Student("Artur", "Bocion", DateTime.Now, repository.IdentityProvider);
            _id = student.Id;
            //_repo.Create(student);
        }

        [HttpGet("{id}", Name = "GetMethodStudent")]
        public async Task<IActionResult> Get(int id)
        {
            var output = await _repo.Read(38);
            return Ok(output);
        }

        /// <summary>
        /// Gets marks (or mark when given mark id) for given student 
        /// </summary>
        /// <param name="id"> Student id </param>
        /// <returns> Student marks/ mark </returns>
        [HttpGet("{id}/marks/{markId:int?}")]
        public IActionResult GetMarksForGivenStudent(int id, int markId)
        {
            try
            {
                var subjects = _entitiesRepository.ReadAll<Subject>();

                var marks = subjects.Aggregate(new List<Mark>(),
                        (list, subject) =>
                        {
                            list.AddRange(subject.Marks.Where(m => m.StudentId == id));
                            return list;
                        });

                if (!marks.Any())
                    return HttpNotFound("Student doesn't have any marks");

                if (markId == 0)
                    return Ok(marks);

                var mark = marks.First(m => m.Id == markId);

                if (mark == null)
                    return HttpNotFound("Student doesn't have mark of given id");

                return Ok(mark);
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }
    }
}