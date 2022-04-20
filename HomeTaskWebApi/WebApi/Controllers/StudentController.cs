using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private static ILogger<StudentController> _logger;
        public StudentController(StudentService studentService, 
            ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        // GET: api/Student
        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> Get()
        {
            _logger.LogInformation("GetAllStudents method is processing");
            return Ok(_studentService.GetAllStudents().Select(student => StudentDto.FromModel(student)));
        }
        // Post api/Student
        [HttpPost]
        public ActionResult Create([FromBody] StudentDto value)
        {
            _logger.LogInformation("CreateStudent method is processing");
            var result = _studentService.CreateStudent(value.ToModel());
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                    _logger.LogError(error.ToString(), " happened in CreateStudent method");
                return BadRequest(result.Errors);
            }
            return Accepted();
        }

        // GET api/Student/5
        [HttpGet("{id}")]
        public ActionResult<StudentDto> Get(int id)
        {
            _logger.LogInformation("GetStudentById method is processing");
            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                _logger.LogWarning("Student ", student.Id, " not found in GetStudentById method");
                return NotFound();
            }

            return Ok(StudentDto.FromModel(student));
        }
        
        // PUT api/Student/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] StudentDto value)
        {
            _logger.LogInformation("UpdateStudent method is processing");
            var result = _studentService.UpdateStudent(value.ToModel());
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                    _logger.LogError(error.ToString(), " happened in UpdateStudent method");
                return BadRequest(result.Errors);
            }
            return Accepted();
        }

        // DELETE api/Student/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("DeleteStudent method is processing");
            _studentService.DeleteStudent(id);
            return Accepted();
        }
    }
}
