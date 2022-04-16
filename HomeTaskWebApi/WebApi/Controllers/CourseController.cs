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
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;
        ILogger<CourseController> _logger;
        public CourseController(CourseService courseService,
             ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }
        // GET: api/Course
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> Get()
        {
            _logger.LogInformation("GetAllCourses method is processing");
            return Ok(_courseService.GetAllCourses().Select(course => CourseDto.FromModel(course)));
        }
        // Post api/Course
        [HttpPost]
        public ActionResult Create([FromBody] CourseDto value)
        {
            _logger.LogInformation("CreateCourse method is processing");
            var result = _courseService.CreateCourse(value.ToModel());
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                    _logger.LogError(error.ToString(), " happened in CreateCourse method");
                return BadRequest(result.Errors);
            }
            return Accepted();
        }

        // GET api/Course/5
        [HttpGet("{id}")]
        public ActionResult<CourseDto> Get(int id)
        {
            _logger.LogInformation("GetCourseById method is processing");
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                _logger.LogWarning("course ", course.Id, " not found in GetCourseById method");
                return NotFound();
            }

            return Ok(CourseDto.FromModel(course));
        }
        
        // PUT api/Course/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CourseDto value)
        {
            _logger.LogInformation("UpdateCourse method is processing");
            var updateResult = _courseService.UpdateCourse(value.ToModel());
            if (updateResult.HasErrors)
            {
                foreach (var error in updateResult.Errors)
                    _logger.LogError(error.ToString(), " happened in UpdateCourse method");
                return BadRequest(updateResult.Errors);
            }
            return Accepted();
        }

        // DELETE api/Course/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("DeleteCourse method is processing");
            _courseService.DeleteCourse(id);
            return Accepted();
        }
    }
}
