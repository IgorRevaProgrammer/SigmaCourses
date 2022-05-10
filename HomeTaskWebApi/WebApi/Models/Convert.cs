using Models.Models;
using System.Linq;
using WebApi.ViewModels;

namespace WebApi.Models
{
    public static class Convert
    {
        public static Course ToCourse(CourseVM courseVM)
        {
            return new Course()
            {
                Id = courseVM.Id,
                Name = courseVM.Name,
                StartDate = courseVM.StartDate,
                EndDate = courseVM.EndDate,
                PassCredits = courseVM.PassCredits,
                HomeTasks = courseVM.HomeTasks.Select(ht => ToHomeTask(ht)).ToList()
            };
        }
        public static HomeTask ToHomeTask(HomeTaskVM homeTaskVM)
        {
            return new HomeTask()
            {
                Id = homeTaskVM.Id,
                Date = homeTaskVM.Date,
                Title = homeTaskVM.Title,
                Description = homeTaskVM.Description,
                Number = homeTaskVM.Number,
                CourseId = homeTaskVM.CourseId
            };
        }
        public static Student ToStudent(StudentVM studentVM)
        {
            return new Student()
            {
                BirthDate = studentVM.BirthDate,
                Id = studentVM.Id,
                Name = studentVM.Name,
                Notes = studentVM.Notes,
                PhoneNumber = studentVM.PhoneNumber,
                Email = studentVM.Email,
                GitHubLink = studentVM.GitHubLink
            };
        }
    }
}
