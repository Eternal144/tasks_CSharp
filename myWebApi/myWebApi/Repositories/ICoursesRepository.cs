using System;
using myWebApi.Models;
namespace myWebApi.Repositories
{
    public interface ICoursesRepository
    {
        int AddNewCourse(Course course);
        int DeleteCourseById(int id);
        //Record GetCourseById(int id);
        //看起来不用做
        Course[] GetCourse();
        int UpdateCourseById(int id, Course course);
        int UpdateCourseByIdEntityState(int id, Course course);
    }
}
