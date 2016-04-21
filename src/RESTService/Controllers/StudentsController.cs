using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System;

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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return HttpNotFound();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var student = _studentsRepository.Retrieve(id);
                return Ok(student);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return HttpNotFound(e);
            }
        }

        // GET: api/students
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var allStudents = _studentsRepository.RetrieveAll();

            if (allStudents != null)
                return Json(allStudents);

            return HttpNotFound();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }
    }
}