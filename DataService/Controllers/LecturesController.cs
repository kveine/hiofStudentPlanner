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
    public class LecturesController : ApiController
    {
        private DataContext db = new DataContext();

        //Class coupling is >10. This code is necessary for many to one relations.
        // GET api/Lectures
        public IQueryable<Lecture> GetLectures()
        {
            return db.Lectures.Include(l => l.Course);
        }

        // GET api/Lectures/5
        [ResponseType(typeof(Lecture))]
        public IHttpActionResult GetLecture(int id)
        {
            Lecture lecture = db.Lectures.Find(id);
            if (lecture == null)
            {
                return NotFound();
            }

            return Ok(lecture);
        }

        // PUT api/Lectures/5
        public IHttpActionResult PutLecture(int id, Lecture lecture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lecture.LectureId)
            {
                return BadRequest();
            }

            db.Entry(lecture).State = EntityState.Modified;

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

        // POST api/Lectures
        [ResponseType(typeof(Lecture))]
        public IHttpActionResult PostLecture(Lecture lecture)
        {
            var courseInLecture = lecture.Course;
            Course course = db.Courses.Find(courseInLecture.CourseId);
            lecture.Course = course;
            ModelState.Clear();

            db.Lectures.Add(lecture);

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = lecture.LectureId }, lecture);
            }
            catch (DataException)
            {
                return BadRequest();
            }
            
        }

        // DELETE api/Lectures/5
        [ResponseType(typeof(Lecture))]
        public IHttpActionResult DeleteLecture(int id)
        {
            Lecture lecture = db.Lectures.Find(id);
            if (lecture == null)
            {
                return NotFound();
            }

            db.Lectures.Remove(lecture);

            try
            {
                db.SaveChanges();
                return Ok(lecture);
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

        //Class coupling is >10. This is generated code.
        private bool LectureExists(int id)
        {
            return db.Lectures.Count(e => e.LectureId == id) > 0;
        }
    }
}