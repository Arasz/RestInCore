using System;
using Microsoft.AspNet.Mvc;
using RESTService.Links;
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
        public async Task<IActionResult> GetMarks(int id,[FromQuery] int value, [FromQuery] DateTime date, [FromQuery] int studentId)
        {
            var subjects = await _subjectRepository.ReadAll().ConfigureAwait(false);

            var marks = subjects.SelectMany(subject => subject.Marks);

            if (value != 0 && date != default(DateTime) && studentId != 0)
                marks = marks.Where(mark1 =>(mark1.SubmitTime.Date == date.Date) && (mark1.Value == value) && (mark1.StudentId == studentId)).ToList();
            if (value != 0 )
                marks = marks.Where(mark1 => (mark1.Value == value)).ToList();
            if ( date != default(DateTime))
                marks = marks.Where(mark1 => (mark1.SubmitTime.Date == date.Date)).ToList();
            if (studentId != 0)
                marks = marks.Where(mark1 => (mark1.StudentId == studentId)).ToList();

            if (!marks.Any())
                return HttpNotFound("There is no mark");

            if (id == 0)
                return Ok(marks);

            var mark = marks.First(m => m.Id == id);

            mark.Resources.AddLinks(
                new Link("Parent", $"/api/marks"),
                new Link("Self", $"/api/marks/{id}"),
                new Link("Next", $"/api/marks/{++id}")
                );

            if (mark == null)
                return HttpNotFound("Mark with given id doesn't exist");

            return Ok(mark);
        }
    }
}