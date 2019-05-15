using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myWebApi.Models
{
    public partial class Record
    {
        [Key]
        public int rid { get; set; }
        public int sid { get; set; }
        public int cid { get; set; }
        public int select_year { get; set; }
        public int scores { get; set; }
    }
}
