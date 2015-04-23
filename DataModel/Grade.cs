using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Grade
    {
        public int GradeId { get; set; }
        [Required]
        public Course Course { get; set; }
        [Required]
        public Student Student { get; set; }
        public GradeValue Value { get; set; }
    }
}
