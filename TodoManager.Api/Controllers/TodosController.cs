using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoManager.Data.Services;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    //[ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILoggerManager _logger;

        public TodosController(ITodoService todoService, ILoggerManager logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoCreateDto createDto)
        {
            try
            {
                var createdTodo = await _todoService.CreateTodo(createDto);
                _logger.LogInfo($"Todo with id {createdTodo.Id} at {createdTodo.CreatedOn}");

                return CreatedAtRoute("GetTodoById", new { id = createdTodo.Id }, createdTodo); //201
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "creating todo");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var todoResponses = await _todoService.GetAllAsync();
                return Ok(todoResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get all todo");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] TodoUpdateDto updateDto)
        {
            try
            {
                await _todoService.UpdateTodo(id, updateDto);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "updating todo");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:long}", Name = "GetTodoById")]
        public async Task<IActionResult> GetTodo(long id)
        {
            try
            {
                var todoResponse = await _todoService.GetTodo(id);
                return Ok(todoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Getting todo by id");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _todoService.Delete(id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
