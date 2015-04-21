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
    public class SubmissionsController : ApiController
    {
        private SchoolEntities db = new SchoolEntities();

        // GET api/Submissions
        public IQueryable<Submission> GetSubmissions()
        {
            return db.Submissions;
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

            return Ok(submission);
        }

        // PUT api/Submissions/5
        public IHttpActionResult PutSubmission(int id, Submission submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            }

            db.Submissions.Add(submission);
            db.SaveChanges();*/
            var courseInSubmission = submission.Course;
            Course course = db.Courses.Find(courseInSubmission.CourseId);
            submission.Course = course;

            var studentInSubmission = submission.Student;
            Student student = db.Students.Find(studentInSubmission.StudentId);
            submission.Student = student;
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