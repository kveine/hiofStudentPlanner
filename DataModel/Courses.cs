using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DataModel
    {
        public class Course
        {
            public Course()
            {
                this.Students = new HashSet<Student>();
                this.Lectures = new HashSet<Lecture>();
            }
            public int CourseId { get; set; }
            [Required]
            [StringLength(50)]
            public string Title { get; set; }
            [StringLength(1000)]
            public string Description { get; set; }
            [Required]
            public Semester Semester { get; set; }
            public Exam Exam { get; set; }
            public ICollection<Lecture> Lectures { get; set; }
            public ICollection<Student> Students { get; set; }
        }
    }

}
