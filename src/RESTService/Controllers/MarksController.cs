using Microsoft.AspNet.Mvc;
using RESTService.Models;
using RESTService.Repository;
using System;
using System.Collections.Generic;

namespace RESTService.Controllers
{
    /// <summary>
    /// Web service controller 
    /// </summary>
    [Route("api/[controller]")]
    public class MarksController : Controller
    {
        /// <summary>
        /// Application data access abstract layer 
        /// </summary>
        private readonly IRepository<Entity> _entitiesRepository;

        public MarksController(IRepository<Entity> entitiesRepository)
        {
            _entitiesRepository = entitiesRepository;
        }

        /// <summary>
        /// Delete mark with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Information about operation state </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var mark = _entitiesRepository.Read(id);
                _entitiesRepository.Delete(mark);

                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get mark with given id number 
        /// </summary>
        /// <param name="id"> Unique id number </param>
        /// <returns> Student with given id number </returns>
        [HttpGet("{id}", Name = "GetMark")]
        public IActionResult Get(int id)
        {
            try
            {
                var mark = _entitiesRepository.Read(id);
                if (mark is StudentMark)
                    return Ok(mark);
                return HttpBadRequest("There is no student mark with this id");
            }
            catch (KeyNotFoundException exception)
            {
                return HttpNotFound(exception);
            }
        }

        /// <summary>
        /// Get all marks 
        /// </summary>
        /// <returns> Students collection </returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var allMarks = _entitiesRepository.ReadAll<StudentMark>();

            if (allMarks != null)
                return Json(allMarks);

            return HttpNotFound();
        }

        /// <summary>
        /// Creates new mark in repository 
        /// </summary>
        /// <param name="mark"> New mark </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]StudentMark mark)
        {
            if (mark == null)
                return HttpBadRequest();

            _entitiesRepository.Create(mark);
            return CreatedAtRoute("GetStudent", new { id = mark.Id }, mark);
        }

        /// <summary>
        /// Updates mark under given id 
        /// </summary>
        /// <param name="id"> Update mark id </param>
        /// <param name="mark"> New mark state </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]StudentMark mark)
        {
            if (mark == null)
                return HttpBadRequest();

            try
            {
                _entitiesRepository.Update(id, mark);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception);
            }
        }
    }
}