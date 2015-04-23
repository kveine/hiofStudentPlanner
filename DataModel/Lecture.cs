using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Lecture
    {
        public int LectureId { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public string Time { get; set; }
        public Course Course { get; set; }
        [Required]
        public string Room { get; set; }
    }
}
