using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labb2.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        public string TeacherName { get; set; }

        public virtual ICollection<StudentSchedule> StudentSchedules { get; set; }

    }
}
