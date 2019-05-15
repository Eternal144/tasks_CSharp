using System;
using myWebApi.Models;
namespace myWebApi.Repositories
{
    public interface IStudentsRepository
    {
        int AddNewStudent(Student student);
        int DeleteStudentById(int id);
        Student[] GetStudents();
        //int UpdateStudentById(int id, Record record);
        int UpdateStudentByIdEntityState(int id, Student student);
    }
}
