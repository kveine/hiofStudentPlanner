using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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

        public ICollection<Course> Courses { get; private set; }

    }

    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
        }
        public int CourseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public int Semester { get; set; }
        public ICollection<Student> Students { get; private set; }
    }

    public class Submission
    {
        public int SubmissionId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }

    public class Exam
    {
        public int ExamId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Room { get; set; }

    }

    public class Grade
    {
        public int GradeId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public string Value { get; set; }

    }

    public class SchoolEntities : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public SchoolEntities()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=kristikv;User ID=kristikv;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;  // Fix One - avoid cycles
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                   .HasMany<Course>(b => b.Courses)
                   .WithMany(a => a.Students)
                   .Map(ab =>
                   {
                       ab.MapLeftKey("StudentRefId");
                       ab.MapRightKey("CourseRefId");
                       ab.ToTable("StudentCourses");
                   });
        }

        /*public System.Data.Entity.DbSet<DataModel.Exam> Exams { get; set; }

        public System.Data.Entity.DbSet<DataModel.Submission> Submissions { get; set; }

        public System.Data.Entity.DbSet<DataModel.Grade> Grades { get; set; }*/
    }
}
