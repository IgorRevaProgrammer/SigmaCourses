using Models.Models;
using System.Linq;

namespace WebApi.ViewModels
{
    public static class ConvertVM
    {
        public static CourseVM ToCourseVM(Course course)
        {
            return new CourseVM()
            {
                Id = course.Id,
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate= course.EndDate,
                PassCredits = course.PassCredits,
                HomeTasks = course.HomeTasks.Select(ht=>ToHomeTaskVM(ht)).ToList()
            };
        }
        public static HomeTaskVM ToHomeTaskVM(HomeTask homeTask)
        {
            return new HomeTaskVM()
            {
                Id = homeTask.Id,
                Date=homeTask.Date,
                Title = homeTask.Title,
                Description=homeTask.Description,
                Number=homeTask.Number,
                CourseId=homeTask.CourseId
            };
        }
        public static StudentVM ToStudentVM(Student student)
        {
            return new StudentVM()
            {
                BirthDate = student.BirthDate,
                Id = student.Id,
                Name = student.Name,
                Notes = student.Notes,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                GitHubLink = student.GitHubLink
            };
        }
    }
}
