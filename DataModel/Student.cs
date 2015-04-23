﻿using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Student
    {
        public Student()
        {
            this.Courses = new HashSet<Course>();
        }
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [StringLength(20)]
        public string Password { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
