using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Exam
    {
        public int ExamId { get; set; }
        [Required]
        public Course Course { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public ExamMethod Method { get; set; }
        [Required]
        public string Date { get; set; }
        public string Room { get; set; }
    }
}
