using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class MarkController : BaseController<StudentMark>
    {
        public MarkController(IRepository<Entity> entitiesRepository) : base(entitiesRepository)
        {
        }

        public override IActionResult Delete()
        {
            return HttpBadRequest("Wrong request");
        }

        public override IActionResult Delete(int id)
        {
            return HttpBadRequest("Wrong request");
        }

        public override IActionResult Post(StudentMark entity)
        {
            return HttpBadRequest("Wrong request");
        }

        public override IActionResult Put(int id, StudentMark entity)
        {
            return HttpBadRequest("Wrong request");
        }
    }
}