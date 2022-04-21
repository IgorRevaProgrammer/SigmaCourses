using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;

namespace WebApi
{
    public class CourseRepository : IRepository<Course>
    {
        UniversityContext context;
        public CourseRepository(UniversityContext _context) 
        {
            context = _context;
        }
        public List<Course> GetAll()
        {
            return (context.Courses
                .Include(c => c.HomeTasks)
                .Include(c => c.Students)
                .ToList());
        }
        public Course GetById(int id)
        {
            return context.Courses.FirstOrDefault(c => c.Id == id);
        }
        public Course Create(Course course)
        {
            context.Courses.Add(course);
            context.SaveChanges();
            return course;
        }
        public void Update(Course course)
        {
            var courseForUpdate = context.Courses.FirstOrDefault(c => c.Id == course.Id);
            if(courseForUpdate != null)
            {
                courseForUpdate.Name = course.Name;
                courseForUpdate.StartDate = course.StartDate;
                courseForUpdate.EndDate = course.EndDate;
                courseForUpdate.PassCredits = course.PassCredits;
                courseForUpdate.Students.Clear();
                courseForUpdate.Students.AddRange(course.Students);
                context.SaveChanges();
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.Courses.FirstOrDefault(s => s.Id == id));
            context.SaveChanges();
        }
    }
}
