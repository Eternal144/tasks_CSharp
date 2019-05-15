using System;
using System.Linq;
using myWebApi.Models;
namespace myWebApi.Repositories
{
    public class StudentRepository : IStudentsRepository
    {
        private IsakilaContext _context;
        public StudentRepository(IsakilaContext context)
        {
            _context = context;
        }
        public int AddNewStudent(Student student)
        {
            int insertSuccess = 0;
            int maxId = _context.Students.Max(p => p.sid);
            student.sid = (short)(maxId + 1);
            _context.Students.Add(student);
            insertSuccess = _context.SaveChanges();
            return insertSuccess;
        }

        public int DeleteStudentById(int id)
        {
            int deleteSuccess = 0;
            var student = _context.Students.SingleOrDefault(a => a.sid == id);
            if(student != null)
            {
                _context.Students.Remove(student);
                deleteSuccess = _context.SaveChanges();
            }
            return deleteSuccess;

        }

        public Student[] GetStudents()
        {
            return _context.Students.ToArray();
 
        }

        public int UpdateStudentByIdEntityState(int id, Student student)
        {
            int updateSuccess = 0;
            if(id != student.sid)
            {
                return updateSuccess;
            }
            _context.MarkAsModified(student);
            updateSuccess = _context.SaveChanges();
            return updateSuccess;

        }
    }
}
