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
            this.Lectures = new ObservableCollection<Lecture>();
        }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Semester { get; set; }
        public Exam Exam { get; set; }
        public ObservableCollection<Lecture> Lectures { get; set; }
        public ObservableCollection<Student> Students { get; set; }
    }

    public class Lecture
    {
        public int LectureId { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime Time { get; set; }
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
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }

    public class Exam
    {
        public int ExamId { get; set; }
        public Course Course { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Room { get; set; }

    }

    public class Grade
    {
        public int GradeId { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
        public string Value { get; set; }

    }
}
