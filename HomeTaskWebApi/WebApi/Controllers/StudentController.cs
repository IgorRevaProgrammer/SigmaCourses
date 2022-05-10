using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Services;
using Microsoft.Extensions.Logging;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private static ILogger<StudentController> _logger;
        IAuthorizationService _authorizationService;
        public StudentController(StudentService studentService,
            IAuthorizationService authorizationService,
            ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            _logger.LogInformation("GetAllStudents method is processing");
            return View((await _studentService.GetAllStudents())
                .Select(s => ConvertVM.ToStudentVM(s))
                .ToList());
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentById(id);
            var result = await _authorizationService.AuthorizeAsync(User, student, "UserAccessPolicy");
            if (result.Succeeded)
            {
                ViewData["Action"] = "Edit";
                return View("Student",ConvertVM.ToStudentVM(student));
            }
            return Forbid();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(StudentVM studentVM)
        {
            _logger.LogInformation("Edit student method is processing");
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("Student", studentVM);
            }
            var result = await _authorizationService.AuthorizeAsync(User, Convert.ToStudent(studentVM), "UserAccessPolicy");
            if (result.Succeeded)
            {
                var validationResult = _studentService.UpdateStudent(Convert.ToStudent(studentVM));
                if (validationResult.HasErrors)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                        _logger.LogError(error.Value, " happened in edit student method");
                    }
                    ViewData["Action"] = "Edit";
                    return View("Student", studentVM);
                }
                return RedirectToAction("GetStudents");
            }
            return Forbid();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            return View("Student",new StudentVM());
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(StudentVM studentVM)
        {
            _logger.LogInformation("CreateStudent method is processing");
            if(!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return View("Student",studentVM);
            }
            var result = await _studentService.CreateStudent(Convert.ToStudent(studentVM));
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.ToString(), " happened in CreateStudent method");
                    ModelState.AddModelError(error.Key,error.Value);
                }
                ViewData["Action"] = "Create";
                return View("Student", studentVM);
            }
            return RedirectToAction("GetStudents");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DeleteStudent method is processing");
            await _studentService.DeleteStudent(id);
            return RedirectToAction("GetStudents");
        }
    }
}
