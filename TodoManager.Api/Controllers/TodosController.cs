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
    [ApiController]
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
            var createdTodo = await _todoService.CreateTodoAsync(createDto);
            _logger.LogInfo($"Todo with id {createdTodo.Id} at {createdTodo.CreatedOn}");

            return CreatedAtRoute("GetTodoById", new { id = createdTodo.Id }, createdTodo); //201
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todoResponses = await _todoService.GetAllAsync();
            return Ok(todoResponses);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] TodoUpdateDto updateDto)
        {
            await _todoService.UpdateTodoAsync(id, updateDto);
            return NoContent(); // 204 
        }

        [HttpGet("{id:long}", Name = "GetTodoById")]
        public async Task<IActionResult> GetTodo(long id)
        {
            var todoResponse = await _todoService.GetByIdAsync(id);
            return Ok(todoResponse);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _todoService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? searchText)
        {
            var todosResponse = await _todoService.SearchAsync(searchText);
            return Ok(todosResponse);
        }
    }
}
