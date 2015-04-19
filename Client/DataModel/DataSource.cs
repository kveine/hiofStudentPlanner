using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.DataModel
{
    class DataSource
    {
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Submission> Submissions { get; set; }
        public ObservableCollection<Submission> Grades { get; set; }
        public ObservableCollection<Student> Student { get; set; }
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
                }
            }
        }

        public static async Task<ObservableCollection<Submission>> GetSubmissionsAsync()
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
        }

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

                    return grades;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<Student>> GetStudentAsync(){
            using(var client = new HttpClient()){
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Students/1");

                if(result.IsSuccessStatusCode){
                    var resultStream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Student>));

                    ObservableCollection<Student> student = (ObservableCollection<Student>)serializer.ReadObject(resultStream);
                    
                    return student;
                } 
                else
                {
                    return null;
                }
            }
        }
    }
}
