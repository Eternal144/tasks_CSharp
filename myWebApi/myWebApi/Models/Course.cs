using System;
using System.ComponentModel.DataAnnotations;
namespace myWebApi.Models
{
    public class Course
    {
        [Key]
        public int cid { get; set; }
        public string course_id { get; set; }
        public string cname { get; set; }
        public string tname { get; set; }
        public int credit { get; set; }
        public int? grade { get; set; }
        public int? cancle_year { get; set; }
    }
}
