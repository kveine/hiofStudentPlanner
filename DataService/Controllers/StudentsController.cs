using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataModel;
using DataAccess;
using DataModel.DataModel;

namespace DataService.Controllers
{
    public class StudentsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Students
        public IQueryable<Student> GetStudents()
        {
            return db.Students.Include(b => b.Courses);
        }

        // GET api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            db.Entry(student).Collection(s => s.Courses).Load();
            return Ok(student);
        }

        // PUT api/Students/5
        public IHttpActionResult PutStudent(int id, Student student)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var student2 = db.Students.Find(student.StudentId);
            db.Entry(student2).Collection(s => s.Courses).Load();

            var courses = student.Courses.ToList<Course>();
            student2.Courses.Clear();

            foreach (var a in courses)
            {
                Course course = db.Courses.Find(a.CourseId);
                student2.Courses.Add(course);
            }

            student2.UserName = student.UserName;
            student2.Password = student.Password;

            if (id != student.StudentId)
            {
                return BadRequest();
            }

            db.Entry(student2).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Students
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(Student student)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var courses = student.Courses.ToList<Course>();
            student.Courses.Clear();

            foreach (var a in courses)
            {
                Course course = db.Courses.Find(a.CourseId);
                student.Courses.Add(course);
            }
            ModelState.Clear();

            db.Students.Add(student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);
        }

        // DELETE api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}