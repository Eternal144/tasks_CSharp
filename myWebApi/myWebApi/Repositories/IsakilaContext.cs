using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace myWebApi.Models
{
    public interface IsakilaContext
    {
        DbSet<Record> Records { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Student> Students { get; set; }


        int SaveChanges();
        EntityEntry Entry(Object entity);

        void MarkAsModified(Record record);
        void MarkAsModified(Course record);
        void MarkAsModified(Student record);
    }
}