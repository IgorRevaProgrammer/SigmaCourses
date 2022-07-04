using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Models;
using Services.Validators;

namespace Services
{
    public class HomeTaskService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<HomeTask> _homeTaskRepository;

        public HomeTaskService()
        {

        }

        public HomeTaskService(IRepository<Course> courseRepository, IRepository<HomeTask> homeTaskRepository)
        {
            _courseRepository = courseRepository;
            _homeTaskRepository = homeTaskRepository;
        }

        public virtual async Task<ValidationResponse<HomeTask>> CreateHomeTask(HomeTask homeTask)
        {
            var response = ValidateHomeTask(homeTask);
            if (response.HasErrors)
            {
                return response;
            }
            var all = await _homeTaskRepository.GetAll();

            if (all.Any(p => p.Title == homeTask.Title))
            {
                return new ValidationResponse<HomeTask>("title", $"HomeTask with title '{homeTask.Title}' already exists.");
            }
            var course = await _courseRepository.GetById(homeTask.CourseId);
            homeTask.Course = course;
            var createdHomeTask = _homeTaskRepository.Create(homeTask);
            return new ValidationResponse<HomeTask>(createdHomeTask);
        }

        public virtual async Task<HomeTask> GetHomeTaskById(int id)
        {
            return await _homeTaskRepository.GetById(id);
        }

        public virtual ValidationResponse UpdateHomeTask(HomeTask homeTask)
        {
            var response = ValidateHomeTask(homeTask);
            if (response.HasErrors)
            {
                return response;
            }
           
            _homeTaskRepository.Update(homeTask);
            return new ValidationResponse();
        }
        public virtual async Task DeleteHomeTask(int homeTaskId)
        {
            var homeTask = await _homeTaskRepository.GetById(homeTaskId);
            if (homeTask == null)
            {
                throw new ArgumentException($"Cannot find homeTask with id '{homeTaskId}'");
            }
            _homeTaskRepository.Remove(homeTaskId);
        }

        public virtual async Task<List<HomeTask>> GetAllHomeTasks()
        {
            return await _homeTaskRepository.GetAll();
        }

        private ValidationResponse<HomeTask> ValidateHomeTask(HomeTask homeTask)
        {
            if (homeTask == null)
            {
                return new ValidationResponse<HomeTask>("homeTask", "HomeTask cannot be null");
            }

            return new ValidationResponse<HomeTask>(homeTask);
        }
    }
}
