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
    public class SubmissionsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Submissions
        public IQueryable<Submission> GetSubmissions()
        {
            return db.Submissions.Include(s => s.Course).Include(s => s.Student);
        }

        // GET api/Submissions/5
        [ResponseType(typeof(Submission))]
        public IHttpActionResult GetSubmission(int id)
        {
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return NotFound();
            }
          
            db.Entry(submission).Reference(s => s.Course).Load();
            db.Entry(submission).Reference(s => s.Student).Load();
            return Ok(submission);
        }

        // PUT api/Submissions/5
        public IHttpActionResult PutSubmission(int id, Submission submission)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            if (id != submission.SubmissionId)
            {
                return BadRequest();
            }

            db.Entry(submission).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
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

        // POST api/Submissions
        [ResponseType(typeof(Submission))]
        public IHttpActionResult PostSubmission(Submission submission)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var courseInSubmission = submission.Course;
            var studentInSubmission = submission.Student;
            Course course = db.Courses.Find(courseInSubmission.CourseId);
            Student student = db.Students.Find(studentInSubmission.StudentId);
            submission.Student = student;
            submission.Course = course;
            ModelState.Clear();

            db.Submissions.Add(submission);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = submission.SubmissionId }, submission);
        }

        // DELETE api/Submissions/5
        [ResponseType(typeof(Submission))]
        public IHttpActionResult DeleteSubmission(int id)
        {
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return NotFound();
            }

            db.Submissions.Remove(submission);
            db.SaveChanges();

            return Ok(submission);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubmissionExists(int id)
        {
            return db.Submissions.Count(e => e.SubmissionId == id) > 0;
        }
    }
}