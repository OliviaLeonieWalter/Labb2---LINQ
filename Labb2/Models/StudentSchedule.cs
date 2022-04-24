using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labb2.Models
{
    public class StudentSchedule
    {
        [Key]
        public int Id { get; set; }


        public int CourseId { get; set; }
        public Course _Course { get; set; }

        public int TeacherId { get; set; }
        public Teacher _Teacher { get; set; }

        public int StudentId{ get; set; }
        public Student _Student { get; set; }
    }
}
