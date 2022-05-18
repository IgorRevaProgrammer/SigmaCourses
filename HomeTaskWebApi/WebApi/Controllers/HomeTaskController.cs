using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Models;
using Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeTaskController : Controller
    {
        private readonly HomeTaskService _homeTaskService;
        private readonly StudentService _studentService;
        private static ILogger<HomeTaskController> _logger;
        public HomeTaskController(HomeTaskService homeTaskService,
            StudentService studentService,
            ILogger<HomeTaskController> logger)
        {
            _studentService = studentService;
            _homeTaskService = homeTaskService;
            _logger = logger;
        }

        [Authorize(Roles="Admin")]
        public IActionResult Create(int courseId)
        {
            ViewData["Action"] = "Create";
            return View("HomeTask", new HomeTaskVM() { CourseId = courseId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(HomeTaskVM homeTaskVM)
        {
            _logger.LogInformation("CreateHomeTask method is processing");
            if(!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return View("HomeTask",homeTaskVM);
            }
            var result = await _homeTaskService.CreateHomeTask(homeTaskVM.ToHomeTask());
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    _logger.LogError(error.ToString(), " happened in CreateHomeTask method");
                }
                ViewData["Action"] = "Create";
                return View("HomeTask", homeTaskVM);
            }
            return RedirectToAction("Edit", "Course", new { id = homeTaskVM.CourseId });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var homeTask= await _homeTaskService.GetHomeTaskById(id);
            if (homeTask == null) return BadRequest();  
            ViewData["Action"] = "Edit";
            return View("HomeTask",homeTask.ToHomeTaskVM());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(HomeTaskVM homeTaskVM)
        {
            if (homeTaskVM == null) return BadRequest();
            _logger.LogInformation("UpdateHomeTask method is processing");
            if(!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("HomeTask",homeTaskVM);
            }
            var updateResult = _homeTaskService.UpdateHomeTask(homeTaskVM.ToHomeTask());
            if (updateResult.HasErrors)
            {
                foreach (var error in updateResult.Errors)
                {
                    _logger.LogError(error.ToString(), " happened in UpdateHomeTask method");
                    ModelState.AddModelError(error.Key, error.Value);
                }
                ViewData["Action"] = "Edit";
                return View("HomeTask", homeTaskVM);
            }
            return RedirectToAction("Edit", "Course", new { id = homeTaskVM.CourseId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id,int courseId)
        {
            _logger.LogInformation("DeleteHomeTask method is processing");
            await _homeTaskService.DeleteHomeTask(id);
            return RedirectToAction("Edit", "Course", new { id = courseId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assessment(int id)
        {
            var homeTask = await _homeTaskService.GetHomeTaskById(id);
            if (homeTask == null) return NotFound();

            var students = (await _studentService.GetAllStudents())
                .Where(students => students.Courses.
                 Where(c => c.Id == homeTask.CourseId).Any()).ToList();

            HomeTaskAssessmentVM homeTaskAssessmentVM =
               new HomeTaskAssessmentVM
               {
                   HomeTaskTitle = homeTask.Title,
                   Date = homeTask.Date,
                   HomeTaskId = homeTask.Id
               };

            foreach(var student in students)
            {
                var homeTaskAssessmentStudentViewModel = new HomeTaskAssessmentStudentViewModel()
                {
                    StudentId = student.Id,
                    StudentName = student.Name
                };
                var homeTaskAssessment = student.HomeTaskAssessments
                    .FirstOrDefault(h => h.HomeTaskId == homeTask.Id);
                if (homeTaskAssessment != null)
                {
                    homeTaskAssessmentStudentViewModel.HomeTaskAssessmentId = homeTaskAssessment.Id;
                    homeTaskAssessmentStudentViewModel.IsComplete = homeTaskAssessment.IsComplete;
                }
                homeTaskAssessmentVM.homeTaskAssessmentStudents.Add(homeTaskAssessmentStudentViewModel);
            }
            return View(homeTaskAssessmentVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assessment(HomeTaskAssessmentVM homeTaskAssessmentVM)
        {
            var homeTask = await _homeTaskService
                .GetHomeTaskById(homeTaskAssessmentVM.HomeTaskId);

            if (homeTask == null) return NotFound();

            foreach (var homeTaskAssessmentStudent in homeTaskAssessmentVM.homeTaskAssessmentStudents)
            {
                var homeTaskAssessment = homeTask.HomeTaskAssessments
                    .FirstOrDefault(p => p.Id == homeTaskAssessmentStudent.HomeTaskAssessmentId);
                if (homeTaskAssessment != null)
                {
                    if (homeTaskAssessment.IsComplete != homeTaskAssessmentStudent.IsComplete)
                    {
                        homeTaskAssessment.Date = DateTime.Now;
                        homeTaskAssessment.IsComplete = homeTaskAssessmentStudent.IsComplete;
                    }
                }
                else
                {
                    homeTask.HomeTaskAssessments.Add(new HomeTaskAssessment
                    {
                        HomeTask = homeTask,
                        IsComplete = homeTaskAssessmentStudent.IsComplete,
                        StudentId = homeTaskAssessmentStudent.StudentId,
                        Date = DateTime.Now
                    });
                }
                _homeTaskService.UpdateHomeTask(homeTask);
            }
            return RedirectToAction("GetCourses", "Course");
        }
    }
}
