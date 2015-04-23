using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        [Required]
        public Course Course { get; set; }
        [Required]
        public Student Student { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public bool Completed { get; set; }
        public string DueDate { get; set; }
    }
}
