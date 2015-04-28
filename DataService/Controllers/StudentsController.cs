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

        //Class coupling is >10, this code is necessary for many to many relations.
        // GET api/Students
        public IQueryable<Student> GetStudents()
        {
            return db.Students.Include(b => b.Courses);
        }

        //Class coupling is >10, this code is necessary for many to many relations.
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

        //Maintainability index <60, class coupling is >10, lines of code is >10. This code is not optimal, but I found no other way to make it work.
        // PUT api/Students/5
        public IHttpActionResult PutStudent(int id, Student student)
        {
            var student2 = db.Students.Find(student.StudentId);
            db.Entry(student2).Collection(s => s.Courses).Load();

            var courses = student.Courses.ToList<Course>();
            student2.Courses.Clear();

            foreach (var a in courses)
            {
                Course course = db.Courses.Find(a.CourseId);
                student2.Courses.Add(course);
            }

            student2.FirstName = student.FirstName;
            student2.LastName = student.LastName;
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
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DataException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        //Maintainability Index is <60, Class coupling is >10. this code is necessary for many to many relations.
        // POST api/Students
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(Student student)
        {
            var courses = student.Courses.ToList<Course>();
            student.Courses.Clear();

            foreach (var a in courses)
            {
                Course course = db.Courses.Find(a.CourseId);
                student.Courses.Add(course);
            }
            ModelState.Clear();

            db.Students.Add(student);
            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);
            }
            catch (DataException)
            {
                return BadRequest();
            }
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

            try
            {
                db.SaveChanges();
                return Ok(student);
            }
            catch (DataException)
            {
                return BadRequest();
            }            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Class coupling is >10, this code is generated.
        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}