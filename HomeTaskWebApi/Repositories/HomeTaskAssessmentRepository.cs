using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Models;
using Models.Models;

namespace WebApi
{
    public class HomeTaskAssessmentRepository : IRepository<HomeTaskAssessment>
    {
        UniversityContext context;
        ILogger<HomeTaskAssessmentRepository> _logger;
        public HomeTaskAssessmentRepository(UniversityContext _context,
            ILogger<HomeTaskAssessmentRepository> logger)
        {
            context = _context;
            _logger = logger;
        }
        public List<HomeTaskAssessment> GetAll()
        {
            return (context.HomeTaskAssessments.ToList());
        }
        public HomeTaskAssessment GetById(int id)
        {
            return context.HomeTaskAssessments.FirstOrDefault(h => h.Id == id);
        }
        public HomeTaskAssessment Create(HomeTaskAssessment homeTaskAssessment)
        {
            context.HomeTaskAssessments.Add(homeTaskAssessment);
            context.SaveChanges();
            return homeTaskAssessment;
        }
        public void Update(HomeTaskAssessment entity)
        {
            var homeTaskAssessmentForUpdate = context.HomeTaskAssessments.FirstOrDefault(h => h.Id == entity.Id);
            if (homeTaskAssessmentForUpdate != null)
            {
                homeTaskAssessmentForUpdate.Date = entity.Date;
                homeTaskAssessmentForUpdate.IsComplete = entity.IsComplete;
                homeTaskAssessmentForUpdate.StudentId = entity.StudentId;
                homeTaskAssessmentForUpdate.HomeTaskId = entity.HomeTaskId;
                context.SaveChanges();
            }
            else
            {
                _logger.LogWarning("HomeTaskAssessment for update came empty to HomeTaskAssessment repository");
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.HomeTaskAssessments.FirstOrDefault(h => h.Id == id));
            context.SaveChanges();
        }
    }
}