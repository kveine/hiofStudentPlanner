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
    public class ExamsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Exams
        public IQueryable<Exam> GetExams()
        {
            return db.Exams;
        }

        // GET api/Exams/5
        [ResponseType(typeof(Exam))]
        public IHttpActionResult GetExam(int id)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }

        // PUT api/Exams/5
        public IHttpActionResult PutExam(int id, Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exam.ExamId)
            {
                return BadRequest();
            }

            db.Entry(exam).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DataException)
            {
                if (!ExamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            
        }

        // POST api/Exams
        [ResponseType(typeof(Exam))]
        public IHttpActionResult PostExam(Exam exam)
        {
            var courseInExam = exam.Course;
            Course course = db.Courses.Find(courseInExam.CourseId);
            exam.Course = course;
            ModelState.Clear();

            db.Exams.Add(exam);
            //db.SaveChanges();

            try
            {
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = exam.ExamId }, exam);
            }
            catch (DataException)
            {
                return BadRequest();
            }
        }

        // DELETE api/Exams/5
        [ResponseType(typeof(Exam))]
        public IHttpActionResult DeleteExam(int id)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            db.Exams.Remove(exam);
            //db.SaveChanges();

            try
            {
                db.SaveChanges();
                return Ok(exam);
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
        private bool ExamExists(int id)
        {
            return db.Exams.Count(e => e.ExamId == id) > 0;
        }
    }
}