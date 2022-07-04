using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Models;

namespace WebApi
{
    public class HomeTaskRepository : IRepository<HomeTask>
    {
        UniversityContext context;
        ILogger<HomeTaskRepository> _logger;
        public HomeTaskRepository(UniversityContext _context,
            ILogger<HomeTaskRepository> logger)
        {
            context = _context;
            _logger = logger;
        }
        public async Task<List<HomeTask>> GetAll()
        {
            return await context.HomeTasks
                .Include(h => h.HomeTaskAssessments)
                .ThenInclude(h=>h.Student)
                .ToListAsync();
        }
        public async Task<HomeTask> GetById(int id)
        {
            return await context.HomeTasks
                .Include(h=>h.HomeTaskAssessments)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
        public HomeTask Create(HomeTask homeTask)
        {
            context.HomeTasks.Add(homeTask);
            context.SaveChanges();
            _logger.LogInformation("HomeTask with title " + homeTask.Title + " created successfully");
            return homeTask;
        }
        public void Update(HomeTask entity)
        {
            if (entity != null)
            {
                context.HomeTasks.Update(entity);
                context.SaveChanges();
                _logger.LogInformation("HomeTask with id " + entity.Id.ToString() + " updated successfully");
            }
            else
            {
                _logger.LogWarning("HomeTask for update came empty to HomeTask repository");
            }
        }
        public void Remove(int id)
        {
            context.Remove(context.HomeTasks.FirstOrDefault(h => h.Id == id));
            context.SaveChanges();
            _logger.LogInformation("HomeTask with id " + id.ToString() + " deleted successfully");
        }
    }
}