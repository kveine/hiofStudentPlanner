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
            this.Lectures = new HashSet<Lecture>();
        }
        public int CourseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public int Semester { get; set; }
        public Exam Exam { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Student> Students { get; set; }
    }
    public class Lecture
    {
        public int LectureId { get; set; }
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public Course Course { get; set; }
        [Required]
        public string Room { get; set; }
    }
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
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }

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

    public class Grade
    {
        public int GradeId { get; set; }
        [Required]
        public Course Course { get; set; }
        [Required]
        public Student Student { get; set; }
        public string Value { get; set; }
    }

    public class SchoolEntities : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public SchoolEntities()
                : base(@"Data Source=donau.hiof.no;Initial Catalog=kristikv;User ID=kristikv;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;  // Fix One - avoid cycles
        }

        //Kode fra Øyvind Øhra, Class Coupling blir 15, vet ikke helt hvordan jeg skal gjøre det annerledes, men skal se mer på det til hovedinnleveringen
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
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

        }
}
