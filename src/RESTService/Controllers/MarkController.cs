using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class MarksController : Controller
    {
        /// <summary>
        /// </summary>
        private readonly IRepository<Subject> _subjectRepository;

        public MarksController(IRepository<Subject> subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        [HttpGet("{id:int?}")]
        public async Task<IActionResult> GetMarks(int id)
        {
            var subjects = await _subjectRepository.ReadAll().ConfigureAwait(false);

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