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
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Room { get; set; }
    }
}
