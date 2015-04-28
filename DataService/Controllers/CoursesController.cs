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
using DataModel.DataModel;
using DataAccess;

namespace DataService.Controllers
{
    public class CoursesController : ApiController
    {
        private DataContext db = new DataContext();

        //Class coupling > 10. This code is generated
        // GET api/Courses
        public IQueryable<Course> GetCourses()
        {
            return db.Courses.Include(c => c.Students).Include(c => c.Exam).Include(c => c.Lectures);
        }

        //Class coupling > 10. This code is generated.
        // GET api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            //db.Entry(course).Collection(b => b.Lectures).Load();
            db.Entry(course).Reference(c => c.Exam).Load();
            return Ok(course);
        }

        // PUT api/Courses/5
        public IHttpActionResult PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // POST api/Courses
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
            }
            catch (DataException)
            {
                return BadRequest();
            }
            
        }

        // DELETE api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            //db.SaveChanges();

            try
            {
                db.SaveChanges();
                return Ok(course);
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

        //Class coupling >10. This is generated code.
        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}