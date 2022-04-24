using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labb2.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int ClassId { get; set; }
        public Class _Class { get; set; }

        public virtual ICollection<StudentSchedule> StudentSchedules { get; set; }
    }
}
