using System.Collections.Generic;
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
        public async Task<List<Course>> GetAll()
        {
            return await context.Courses
                .Include(c => c.HomeTasks)
                .Include(c => c.Students)
                .ToListAsync();
        }
        public async Task<Course> GetById(int id)
        {
            return await context.Courses
                .Include(c=>c.Students)
                .Include(c=>c.HomeTasks)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public Course Create(Course course)
        {
            context.Courses.Add(course);
            context.SaveChanges();
            _logger.LogInformation("Course with name " + course.Name + " created successfully");
            return course;
        }
        public void Update(Course course)
        {
            if(course != null)
            {
                 context.Update(course);
                 context.SaveChanges();
                _logger.LogInformation("Course with id " + course.Id.ToString() + " updated successfully");

            }
            else
            {
                _logger.LogWarning("Course for update came empty to course repository");
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.Courses.FirstOrDefault(s => s.Id == id));
            context.SaveChanges();
            _logger.LogInformation("Course with id " + id.ToString() + " deleted successfully");
        }
    }
}