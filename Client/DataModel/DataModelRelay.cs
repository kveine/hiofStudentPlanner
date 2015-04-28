using Client.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DataModel
{
    public class Student
    {
        public Student()
        {
            this.Courses = new ObservableCollection<Course>();
        }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //Courses can not be read only because I need to update the list. I have tried to use a private setter, but this gives me an error
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<Course> Courses { get; set; }

    }

    public class Course
    {
        public Course()
        {
            this.Students = new ObservableCollection<Student>();
            this.Lectures = new ObservableCollection<Lecture>();
        }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Semester Semester { get; set; }
        public Exam Exam { get; set; }
        //Lectures can not be read only because I need to update the list. I have tried to use a private setter, but this gives me an error
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<Lecture> Lectures { get; set; }
        //Students can not be read only because I need to update the list. I have tried to use a private setter, but this gives me an error
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<Student> Students { get; set; }
    }

    public enum Semester
    {
        Fall = 0,
        Spring
    }

    public class Lecture
    {
        public int LectureId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string Time { get; set; }
        public Course Course { get; set; }
        public string Room { get; set; }
    }
    public class Submission
    {
        public int SubmissionId { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public string DueDate { get; set; }
    }

    public class Exam
    {
        public int ExamId { get; set; }
        public Course Course { get; set; }
        public string Description { get; set; }
        public ExamMethod Method { get; set; }
        public string Date { get; set; }
        public string Room { get; set; }

    }

    public enum ExamMethod
    {
        Project = 0,
        Handwritten
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
        public GradeValue Value { get; set; }
        public string ImagePath { get; set; }

    }
    public enum GradeValue
    {
        //Naming is correct in my case
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "A")]
        A = 0,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "B")]
        B,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "C")]
        C,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        D,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "E")]
        E,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "F")]
        F
    }
}
