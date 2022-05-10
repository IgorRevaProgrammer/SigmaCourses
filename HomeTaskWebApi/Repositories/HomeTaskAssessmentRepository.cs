using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<HomeTaskAssessment>> GetAll()
        {
            return await context.HomeTaskAssessments.ToListAsync();
        }
        public async Task<HomeTaskAssessment> GetById(int id)
        {
            return await context.HomeTaskAssessments
                .Include(h=>h.HomeTask)
                .Include(h=>h.Student)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
        public HomeTaskAssessment Create(HomeTaskAssessment homeTaskAssessment)
        {
            context.HomeTaskAssessments.Add(homeTaskAssessment);
            context.SaveChanges();
            _logger.LogInformation("HomeTaskAssessment with created successfully");
            return homeTaskAssessment;
        }
        public void Update(HomeTaskAssessment entity)
        {
            if (entity != null)
            {
                context.HomeTaskAssessments.Update(entity);
                context.SaveChanges();
                _logger.LogInformation("HomeTaskAssessment with id " + entity.Id.ToString() + " updated successfully");
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
            _logger.LogInformation("HomeTaskAssessment with id " + id.ToString() + " deleted successfully");
        }
    }
}