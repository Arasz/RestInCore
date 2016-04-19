using Microsoft.AspNet.Mvc;
using RESTService.Repository;
using System.Collections.Generic;
using System.Linq;
using RESTService.Models;

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

        public StudentsController(IRepository<Student>  studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _studentsRepository.RetriveAll().Select(student => student.ToString());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value" + id;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}