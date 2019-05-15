using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Models;
using myWebApi.Repositories;

namespace mywebApi.Controllers.v1
{
    [Route("api/[controller]")]
    public class GradesController : Controller
    {
        private IRecordsRepository records;
        private IStudentsRepository students;
        private ICoursesRepository courses;
        public GradesController(sakilaContext context)
        {
            records = new RecordsRepository(context);
            students = new StudentRepository(context);
            courses = new CoursesRepository(context);
        }

        //通过学生学号和课程编号查询,
        [HttpGet]
        public IActionResult Get([FromQuery]string sname, [FromQuery]string cname)
        {
            //先找到sid和cid
            string aa = sname;
            string bb = cname;
            return BadRequest();
        }
        [Route("{flag:int}")]
        [HttpGet ]
        public IActionResult Get([FromQuery]int sid,[FromQuery]string cname)
        {
            int aa = sid;
            string k = sid.ToString();
            string bb = cname;
            string cc = aa + bb;
            return BadRequest();
        }
    }
}
