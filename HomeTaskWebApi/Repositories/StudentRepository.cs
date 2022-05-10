using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Models;

namespace WebApi
{
    public class StudentRepository : IRepository<Student>
    {
        UniversityContext context;
        ILogger<StudentRepository> _logger;
        public StudentRepository(UniversityContext _context,
                   ILogger<StudentRepository> logger)
        {
            context = _context;
            _logger = logger;
        }
        public async Task<List<Student>> GetAll()
        {
            return await context.Students
                .Include(s => s.HomeTaskAssessments)
                .Include(s => s.Courses)
                .ToListAsync();
        }
        public async Task<Student> GetById(int id)
        {
            return await context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public Student Create(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            _logger.LogInformation("Student with email " + student.Email + " created successfully");
            return student;
        }
        public void Update(Student entity)
        { 
            if (entity != null) 
            {
                context.Update(entity);
                context.SaveChanges();
                _logger.LogInformation("Student with id " + entity.Id.ToString() + " updated successfully");
            }
            else
            {
                _logger.LogWarning("Student for update came empty to student repository");
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.Students.FirstOrDefault(s => s.Id == id));
            context.SaveChanges();
            _logger.LogInformation("Student with id " + id.ToString() + " deleted successfully");
        }
    }
}