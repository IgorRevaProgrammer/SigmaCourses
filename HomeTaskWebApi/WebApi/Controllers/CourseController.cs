using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Services;
using Microsoft.Extensions.Logging;
using WebApi.ViewModels;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly StudentService _studentService;
        ILogger<CourseController> _logger;
        public CourseController(CourseService courseService,
            StudentService studentService,
             ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            _logger.LogInformation("GetCourses method is processing");
            return View((await _courseService
                .GetAllCourses())
                .Select(c => ConvertVM.ToCourseVM(c))
                .ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            return View("Course",new CourseVM());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseVM courseVM)
        {
            _logger.LogInformation("CreateCourse method is processing");
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return View("Course", courseVM);
            }

            var result = await _courseService.CreateCourse(Convert.ToCourse(courseVM));
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    _logger.LogError(error.ToString(), " happened in CreateCourse method");
                }
                ViewData["Action"] = "Create";
                return View("Course", courseVM);
            }
            return RedirectToAction("GetCourses");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null) return NotFound();
            ViewData["Action"] = "Edit";
            return View("Course",ConvertVM.ToCourseVM(course));
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult Edit(CourseVM courseVM)
        {
            _logger.LogInformation("Edit course method is processing");
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("Course", courseVM);
            }

            var result = _courseService.UpdateCourse(Convert.ToCourse(courseVM));
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    _logger.LogError(error.ToString(), " happened in edit course method");
                }
                ViewData["Action"] = "Edit";
                return View("Course", courseVM);
            }
            return RedirectToAction("GetCourses");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudentsToCourse(int id)
        {
            var students = await _studentService.GetAllStudents();
            if (students == null) return StatusCode(500);

            var course = await _courseService.GetCourseById(id);
            if (course == null) return BadRequest();

            AddStudentsToCourseVM addStudentsToCourseVM = new AddStudentsToCourseVM()
            {
                Id=course.Id,
                Name=course.Name,
            };

            foreach (var student in students)
            {
                bool isAssigned = course.Students.Any(c => c.Id == student.Id);
                addStudentsToCourseVM.students.Add(
                    new AssignmentStudentVM() 
                    { 
                        Id = student.Id, 
                        Name = student.Name, 
                        IsAssigned = isAssigned 
                    });
            }

            return View(addStudentsToCourseVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudentsToCourse(AddStudentsToCourseVM addStudentsToCourseVM)
        {
            _logger.LogInformation("AddStudentsToCourse method is processing");
            await _courseService.SetStudentsToCourse(addStudentsToCourseVM.Id, 
                addStudentsToCourseVM.students
                .Where(p => p.IsAssigned).Select(student => student.Id));

            return RedirectToAction("GetCourses");
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DeleteCourse method is processing");
            await  _courseService.DeleteCourse(id);
            return RedirectToAction("GetCourses");
        }
    }
}
