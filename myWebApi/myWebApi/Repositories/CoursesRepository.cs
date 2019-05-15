using myWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace myWebApi.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private IsakilaContext _context;

        public CoursesRepository(IsakilaContext context)
        {
            _context = context;
        }

        public int AddNewCourse(Course course)
        {
            int insertSuccess = 0;
            int maxId = _context.Records.Max(p => p.cid);
            course.cid = (short)(maxId + 1);
            _context.Courses.Add(course);
            insertSuccess = _context.SaveChanges();
            return insertSuccess;

        }

        public int DeleteCourseById(int id)
        {
            int deleteSuccess = 0;
            var course = _context.Courses.SingleOrDefault(a => a.cid == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                deleteSuccess = _context.SaveChanges();
            }
            return deleteSuccess;
        }

        public Course[] GetCourse()
        {
            return _context.Courses.ToArray();
        }

        public int UpdateCourseById(int id, Course course)
        {
            int updateSuccess = 0;
            var target = _context.Courses.SingleOrDefault(a => a.cid == id);
            if (target != null)
            {
                _context.Entry(target).CurrentValues.SetValues(course);
                updateSuccess = _context.SaveChanges();
            }
            return updateSuccess;
        }

        public int UpdateCourseByIdEntityState(int id, Course course)
        {
            int updateSuccess = 0;
            if (id != course.cid)
            {
                return updateSuccess;
            }
            _context.MarkAsModified(course);
            updateSuccess = _context.SaveChanges();
            return updateSuccess;
        }
    }
}
