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

        public ObservableCollection<Course> Courses { get; private set; }

    }

    public class Course
    {
        public Course()
        {
            this.Students = new ObservableCollection<Student>();
        }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Semester { get; set; }
        public ObservableCollection<Student> Students { get; private set; }
    }

    public class Submission
    {
        public int SubmissionId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }

    public class Exam
    {
        public int ExamId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Room { get; set; }

    }

    public class Grade
    {
        public int GradeId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Value { get; set; }

    }
}
