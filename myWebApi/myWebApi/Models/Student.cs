using System;
using System.ComponentModel.DataAnnotations;
namespace myWebApi.Models
{
    public class Student
    {
        [Key]
        public int sid { get; set; }
        public string student_id { get; set; }
        public string sname { get; set; }
        public string gender { get; set; }
        public int adm_age { get; set; }
        public int adm_year { get; set; }
        public int? classroom { get; set; }
    }
}
