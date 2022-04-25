using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
        public List<Student> GetAll()
        {
            return (context.Students
                .Include(s => s.HomeTaskAssessments)
                .Include(s => s.Courses)
                .ToList());
        }
        public Student GetById(int id)
        {
            return context.Students.FirstOrDefault(s => s.Id == id);
        }
        public Student Create(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }
        public void Update(Student entity)
        {
            var studentForUpdate = context.Students.FirstOrDefault(s => s.Id == entity.Id);
            if (studentForUpdate != null)
            {
                studentForUpdate.Courses = studentForUpdate.Courses;
                studentForUpdate.Email = studentForUpdate.Email;
                studentForUpdate.GitHubLink = studentForUpdate.GitHubLink;
                studentForUpdate.Name = studentForUpdate.Name;
                studentForUpdate.BirthDate = studentForUpdate.BirthDate;
                studentForUpdate.PhoneNumber = studentForUpdate.PhoneNumber;
                studentForUpdate.Notes = studentForUpdate.Notes;
                context.SaveChanges();
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
        }
    }
}