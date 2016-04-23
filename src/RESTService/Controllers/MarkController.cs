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
    public class MarksController : Controller
    {
        private IRepository<Entity> _repository;

        public MarksController(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:int?}")]
        public IActionResult GetMarks(int id)
        {
            var subjects = _repository.ReadAll<Subject>();

            var marks = subjects.Aggregate(new List<Mark>(),
                (list, subject) =>
                {
                    list.AddRange(subject.Marks);
                    return list;
                });

            if (!marks.Any())
                return HttpNotFound("There is no mark");

            if (id == 0)
                return Ok(marks);

            var mark = marks.First(m => m.Id == id);

            if (mark == null)
                return HttpNotFound("Mark with given id doesn't exist");

            return Ok(mark);
        }
    }
}