﻿using System;
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
    public class GradesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Grades
        public IQueryable<Grade> GetGrades()
        {
            return db.Grades.Include(g => g.Student).Include(g => g.Course);
        }

        // GET api/Grades/5
        [ResponseType(typeof(Grade))]
        public IHttpActionResult GetGrade(int id)
        {
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        // PUT api/Grades/5
        public IHttpActionResult PutGrade(int id, Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grade.GradeId)
            {
                return BadRequest();
            }

            db.Entry(grade).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
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

        // POST api/Grades
        [ResponseType(typeof(Grade))]
        public IHttpActionResult PostGrade(Grade grade)
        {
            var courseInGrades = grade.Course;
            var studentInGrades = grade.Student;
            Course course = db.Courses.Find(courseInGrades.CourseId);
            Student student = db.Students.Find(studentInGrades.StudentId);
            grade.Student = student;
            grade.Course = course;
            ModelState.Clear();

            db.Grades.Add(grade);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = grade.GradeId }, grade);
        }

        // DELETE api/Grades/5
        [ResponseType(typeof(Grade))]
        public IHttpActionResult DeleteGrade(int id)
        {
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return NotFound();
            }

            db.Grades.Remove(grade);
            db.SaveChanges();

            return Ok(grade);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GradeExists(int id)
        {
            return db.Grades.Count(e => e.GradeId == id) > 0;
        }
    }
}