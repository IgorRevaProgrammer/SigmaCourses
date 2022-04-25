using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Models;

namespace WebApi
{
    public class CourseRepository : IRepository<Course>
    {
        UniversityContext context;
        ILogger<CourseRepository> _logger;
        public CourseRepository(UniversityContext _context,
             ILogger<CourseRepository> logger)
        {
            context = _context;
            _logger = logger;
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
            if (courseForUpdate != null)
            {
                courseForUpdate.Name = course.Name;
                courseForUpdate.StartDate = course.StartDate;
                courseForUpdate.EndDate = course.EndDate;
                courseForUpdate.PassCredits = course.PassCredits;
                if(course.Students != null)
                {
                    courseForUpdate.Students.Clear();
                    courseForUpdate.Students.AddRange(course.Students);
                }
                context.SaveChanges();
            }
            else{
                _logger.LogWarning("Course for update came empty to course repository");
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.Courses.FirstOrDefault(s => s.Id == id));
            context.SaveChanges();
        }
    }
}