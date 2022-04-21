using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models;
using Models.Models;

namespace WebApi
{
    public class HomeTaskAssessmentRepository : IRepository<HomeTaskAssessment>
    {
        UniversityContext context;
        public HomeTaskAssessmentRepository(UniversityContext _context)
        {
            context = _context;
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
        }
        public void Remove(int id)
        {
            context.Remove(context.HomeTaskAssessments.FirstOrDefault(h => h.Id == id));
            context.SaveChanges();
        }
    }
}
