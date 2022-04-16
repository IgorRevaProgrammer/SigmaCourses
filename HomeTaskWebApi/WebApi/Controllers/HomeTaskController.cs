using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System.Collections.Generic;
using System.Linq;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeTaskController : ControllerBase
    {
        private readonly HomeTaskService _homeTaskService;
        private static ILogger<HomeTaskController> _logger;
        public HomeTaskController(HomeTaskService homeTaskService,
            ILogger<HomeTaskController> logger)
        {
            _homeTaskService = homeTaskService;
            _logger = logger;
        }
        // GET: api/HomeTask
        [HttpGet]
        public ActionResult<IEnumerable<HomeTaskDto>> Get()
        {
            _logger.LogInformation("GetAllHomeTasks method is processing");
            return Ok(_homeTaskService.GetAllHomeTasks().Select(homeTask => HomeTaskDto.FromModel(homeTask)));
        }
        // Post api/HomeTask
        [HttpPost]
        public ActionResult Create([FromBody] HomeTaskDto value)
        {
            _logger.LogInformation("CreateHomeTask method is processing");
            var result = _homeTaskService.CreateHomeTask(value.ToModel());
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                    _logger.LogError(error.ToString(), " happened in CreateHomeTask method");
                return BadRequest(result.Errors);
            }
            return Accepted();
        }

        // GET api/HomeTask/5
        [HttpGet("{id}")]
        public ActionResult<HomeTaskDto> Get(int id)
        {
            _logger.LogInformation("GetHomeTaskById method is processing");
            var homeTask = _homeTaskService.GetHomeTaskById(id);

            if (homeTask == null)
            {
                _logger.LogWarning("HomeTask ", homeTask.Id, " not found in GetHomeTaskById method");
                return NotFound();
            }

            return Ok(HomeTaskDto.FromModel(homeTask));
        }

        // PUT api/HomeTask/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HomeTaskDto value)
        {
            _logger.LogInformation("UpdateHomeTask method is processing");
            var updateResult = _homeTaskService.UpdateHomeTask(value.ToModel());
            if (updateResult.HasErrors)
            {
                foreach (var error in updateResult.Errors)
                    _logger.LogError(error.ToString(), " happened in UpdateHomeTask method");
                return BadRequest(updateResult.Errors);
            }
            return Accepted();
        }

        // DELETE api/HomeTask/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("DeleteHomeTask method is processing");
            _homeTaskService.DeleteHomeTask(id);
            return Accepted();
        }
    }
}
