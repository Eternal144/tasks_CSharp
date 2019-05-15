using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Models;
using myWebApi.Repositories;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private ICoursesRepository courses;
        public CourseController(sakilaContext context)
        {
            courses = new CoursesRepository(context);
        }
        //GET api/v1/courses
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(courses.GetCourse());
        }

        //POSt api/v1/courses
        [HttpPost]
        public IActionResult Post([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int success = courses.AddNewCourse(course);
            if (success == 1)
            {
                return Created("api/v1/records", course);
            }
            return BadRequest();
        }

        //最后这个字段要加在url中。
        //PUT api/v1/courses
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int success = courses.UpdateCourseByIdEntityState(id, course);
            if (success == 1)
            {
                return Ok();
            }
            return BadRequest();
        }


        //DELETE api/v1/courses
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int success = courses.DeleteCourseById(id);
            if(success == 1)
            {
                return Ok();
            }
            return BadRequest();
        }


    }
}
