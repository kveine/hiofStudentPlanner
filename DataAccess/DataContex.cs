using DataModel;
using DataModel.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DataContext()
                : base(@"Data Source=donau.hiof.no;Initial Catalog=kristikv;User ID=kristikv;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;  // Fix One - avoid cycles - Code sample from Øyvind Øhra
        }

        //Class coupling is >10, but this code is neccessary to provide a M:M relation
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
