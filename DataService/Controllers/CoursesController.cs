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

namespace DataService.Controllers
{
    public class CoursesController : ApiController
    {
        private SchoolEntities db = new SchoolEntities();

        // GET api/Courses
        public IQueryable<Course> GetCourses()
        {
            return db.Courses.Include(b => b.Students); // Fix Three - loads related objects (but not cycles)
        }

        // GET api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            db.Entry(course).Collection(b => b.Lectures).Load(); // Fix Three - loads related objects (but not cycles)
            return Ok(course);
        }

        // PUT api/Courses/5
        public IHttpActionResult PutCourse(int id, Course course)
        {
            /*var lectures = course.Lectures.ToList<Lecture>();
            course.Lectures.Clear();

            foreach (var a in lectures)
            {
                Lecture lecture = db.Lectures.Find(a.LectureId);
                course.Lectures.Add(lecture);
            }
            ModelState.Clear();

            db.Courses.Add(course);
            db.SaveChanges();*/
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
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST api/Courses
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
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
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}