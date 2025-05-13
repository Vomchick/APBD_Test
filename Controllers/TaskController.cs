using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Test1.Contracts.Requests;
using Test1.Contracts.Responses;
using Test1.Exceptions;
using Test1.Services.Core;

namespace Test1.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TeamMemberResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamMember(int id)
        {
            try
            {
                var visit = await _taskService.GetTeamMemberAsync(id);
                return Ok(visit);
            }
            catch (TeamMemberNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskRequest request)
        {
            try
            {
                var inserted = await _taskService.AddTaskAsync(request);
                return Ok(inserted);
            }
            catch (TaskTypeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
