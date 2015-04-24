using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization;

namespace Client.DataModel
{
    //Jeg har prøvd å lage en private constructor som foreslått i warningen, men det fungerer ikke
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    class DataSource
    {
        private DataSource() { }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Submission> Submissions { get; set; }
        public ObservableCollection<Grade> Grades { get; set; }
        public ObservableCollection<Course> StudentCourses { get; set; }
        
        
        public static async Task<ObservableCollection<Student>> GetStudentsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Students");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Student>));

                    ObservableCollection<Student> students = (ObservableCollection<Student>)serializer.ReadObject(resultSTream);

                    return students;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<Student> GetStudentAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Students/" + id);

                if (result.IsSuccessStatusCode)
                {
                    var resultStream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(Student));
                    Student student = (Student)serializer.ReadObject(resultStream);

                    return student;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task AddStudentAsync(string firstname, string lastname, string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Student newStudent = new Student() { FirstName = firstname, LastName = lastname, UserName = username, Password = password };
                var jsonSerializer = new DataContractJsonSerializer(typeof(Student));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newStudent);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Students", content);

                response.EnsureSuccessStatusCode();
            }
        }

        public static async Task UpdateStudentAync(Student updatedStudent, int currentStudent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonSerializer = new DataContractJsonSerializer(typeof(Student));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, updatedStudent);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/Students" + updatedStudent.StudentId, content);

                response.EnsureSuccessStatusCode();
            }
        }

        public static async Task<ObservableCollection<Course>> GetStudentCoursesAsync(int currentStudent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Courses");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Course>));

                    ObservableCollection<Course> courses = (ObservableCollection<Course>)serializer.ReadObject(resultSTream);

                    ObservableCollection<Course> studentCourses = new ObservableCollection<Course>();
                    foreach (var entry in courses)
                    {
                        foreach (var student in entry.Students)
                        {
                            if (student.StudentId == currentStudent)
                            {
                                studentCourses.Add(entry);
                            }
                        }
                    }
                    return studentCourses;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the courses asynchronous.
        /// </summary>
        /// <returns>Observable collection of courses</returns>
        public static async Task<ObservableCollection<Course>> GetCoursesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Courses");
                result.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

                const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
                var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Course>), jsonSerializerSettings);
                var stream = await result.Content.ReadAsStreamAsync();

                ObservableCollection<Course> courses = (ObservableCollection<Course>)jsonSerializer.ReadObject(stream);

                return courses;
            }
        }

        public static async Task<ObservableCollection<Grade>> GetGradesAsync(int currentStudent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Grades");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Grade>));

                    ObservableCollection<Grade> grades = (ObservableCollection<Grade>)serializer.ReadObject(resultSTream);
                    ObservableCollection<Grade> studentGrades = new ObservableCollection<Grade>();

                    foreach(var grade in grades){
                        if(grade.Student.StudentId == currentStudent){
                            studentGrades.Add(grade);
                        }
                    }

                    return studentGrades;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task AddGradeAsync(GradeValue value, Course course, Student student, string imagePath)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Grade newGrade = new Grade() { Course = course, Student = student, Value = value, ImagePath = imagePath};
                var jsonSerializer = new DataContractJsonSerializer(typeof(Grade));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newGrade);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Grades", content);

                response.EnsureSuccessStatusCode();
            }
        }

        public static async Task DeleteGradeAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.DeleteAsync("api/Grades/" + id);

                result.EnsureSuccessStatusCode();


            }
        }

        public static async Task<ObservableCollection<Lecture>> GetLecturesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Lectures");
                result.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

                const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
                var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Lecture>), jsonSerializerSettings);
                var stream = await result.Content.ReadAsStreamAsync();

                ObservableCollection<Lecture> lectures = (ObservableCollection<Lecture>)jsonSerializer.ReadObject(stream);

                return lectures;
            }
        }

        public static async Task<ObservableCollection<Submission>> GetSubmissionsAsync(int currentStudent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Submissions");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Submission>));

                    ObservableCollection<Submission> submissions = (ObservableCollection<Submission>)serializer.ReadObject(resultSTream);
                    ObservableCollection<Submission> studentSubmissions = new ObservableCollection<Submission>();

                    foreach (var submission in submissions)
                    {
                        if (submission.Student.StudentId == currentStudent)
                        {
                            studentSubmissions.Add(submission);
                        }
                    }

                    return studentSubmissions;
                }
                else
                {
                    return null;
                }
            }
        }


        public static async Task AddSubmissionAsync(string title, Course course, Student student, string description, string dueDate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Submission newGrade = new Submission() { Title = title, Course = course, Student = student, Description = description, DueDate = dueDate, Completed = false };
                var jsonSerializer = new DataContractJsonSerializer(typeof(Submission));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newGrade);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Submissions", content);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
