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
        public ObservableCollection<Submission> Grades { get; set; }
        public ObservableCollection<Student> Student { get; set; }
        public ObservableCollection<Course> StudentCourses { get; set; }
        
        public static async Task<ObservableCollection<Course>> GetStudentCoursesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Students/1");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Student>));

                    ObservableCollection<Student> students = (ObservableCollection<Student>)serializer.ReadObject(resultSTream);
                    ObservableCollection<Course> studentCourses = new ObservableCollection<Course>();
                    foreach (var entry in students)
                    {
                        foreach (var course in entry.Courses)
                        {
                            studentCourses.Add(course);
                            Debug.WriteLine(course.Title);
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

        public static async Task<ObservableCollection<Course>> GetCoursesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Courses");
                result.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

                const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";
                var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Course>), jsonSerializerSettings);
                var stream = await result.Content.ReadAsStreamAsync();

                ObservableCollection<Course> courses = (ObservableCollection<Course>)jsonSerializer.ReadObject(stream);

                return courses;
                //return (Course)jsonSerializer.ReadObject(stream);
                /*
                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Course>));

                    ObservableCollection<Course> courses = (ObservableCollection<Course>)serializer.ReadObject(resultSTream);

                    return courses;
                }
                else
                {
                    return null;
                }*/
            }
        }

        /*public static async Task<ObservableCollection<Submission>> GetSubmissionsAsync()
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

                    return submissions;
                }
                else
                {
                    return null;
                }
            }
        }*/

        public static async Task<ObservableCollection<Grade>> GetGradesAsync()
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
                    /*ObservableCollection<Grade> studentGrades = new ObservableCollection<Grade>();

                    for(var grade in grades){
                        if(grade.student.id == currentStudent){
                            studentGrades.Add(grade);
                        }
                    }*/

                    return grades;
                }
                else
                {
                    return null;
                }
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

                //const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffzz";
                //var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Lecture>));//, jsonSerializerSettings
                var stream = await result.Content.ReadAsStreamAsync();

                ObservableCollection<Lecture> lectures = (ObservableCollection<Lecture>)jsonSerializer.ReadObject(stream);

                return lectures;


                /*if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Grade>));

                    ObservableCollection<Grade> grades = (ObservableCollection<Grade>)serializer.ReadObject(resultSTream);

                    return grades;
                }
                else
                {
                    return null;
                }*/
            }
        }

        public static async Task<ObservableCollection<Student>> GetStudentAsync(int id)
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
                    Student student1 = (Student)serializer.ReadObject(resultStream);

                    ObservableCollection<Student> student = new ObservableCollection<Student>();
                    student.Add(student1);

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

        public static async Task AddGradeAsync(String value, Course course, Student student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Grade newGrade = new Grade() { Course = course, Student = student, Value = value };
                var jsonSerializer = new DataContractJsonSerializer(typeof(Grade));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newGrade);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Grades", content);

                response.EnsureSuccessStatusCode();
            }
        }

        /*public static async void DeleteGrade(){
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newStudent);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Students", content);

                response.EnsureSuccessStatusCode();
            }
        }*/
    }
}
