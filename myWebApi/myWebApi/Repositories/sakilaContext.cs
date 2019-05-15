using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace myWebApi.Models
{
    public partial class sakilaContext : DbContext, IsakilaContext
    {
        public sakilaContext()
        {
        }

        public sakilaContext(DbContextOptions<sakilaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Record>Records { get; set; }
        public DbSet<Course> Courses { get ; set ; }
        public DbSet<Student> Students { get ; set ; }

        // Add this method for unit testing update
        public void MarkAsModified(Record record)
        {
            Entry(record).State = EntityState.Modified;
        }

        public void MarkAsModified(Course course)
        {
            Entry(course).State = EntityState.Modified;
        }

        public void MarkAsModified(Student student)
        {
            Entry(student).State = EntityState.Modified;
        }

    }
}
