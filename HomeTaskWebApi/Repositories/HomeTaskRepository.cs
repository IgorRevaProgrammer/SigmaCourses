using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;

namespace WebApi
{
    public class HomeTaskRepository : IRepository<HomeTask>
    {
        UniversityContext context;
        public HomeTaskRepository(UniversityContext _context)
        {
            context = _context;
        }
        public List<HomeTask> GetAll()
        {
            return (context.HomeTasks
                .Include(h => h.HomeTaskAssessments)
                .ToList());
        }
        public HomeTask GetById(int id)
        {
            return context.HomeTasks.FirstOrDefault(h => h.Id == id);
        }
        public HomeTask Create(HomeTask homeTask)
        {
            context.HomeTasks.Add(homeTask);
            context.SaveChanges();
            return homeTask;
        }
        public void Update(HomeTask entity)
        {
            var homeTaskForUpdate = context.HomeTasks.FirstOrDefault(h => h.Id == entity.Id);
            if (homeTaskForUpdate != null)
            {
                homeTaskForUpdate.HomeTaskAssessments.Clear();
                homeTaskForUpdate.HomeTaskAssessments.AddRange(entity.HomeTaskAssessments);
                homeTaskForUpdate.Title = entity.Title;
                homeTaskForUpdate.Description = entity.Description;
                homeTaskForUpdate.Date = entity.Date;
                homeTaskForUpdate.Number = entity.Number;
                context.SaveChanges();
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.HomeTasks.FirstOrDefault(h => h.Id == id));
            context.SaveChanges();
        }
    }
}
