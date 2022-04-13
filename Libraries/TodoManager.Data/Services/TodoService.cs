using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoManager.Data.Entities;
using TodoManager.Membership.Entities;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Data.Services;

public class TodoService : ITodoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public readonly ITodoRepositoryManager _repository;

    public TodoService(
        ITodoRepositoryManager repository, 
        IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task CreateTodo(TodoCreateDto createDto)
    {
        var todo = new Todo()
        {
            Description = createDto.Description,
            IsDone = false,
            CreatedBy = (await GetLoggedInUserAsync()).Id,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now
        };

        await _repository.Todos.Create(todo);
        await _repository.SaveChanges();
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
    {
        IEnumerable<TodoResponseDto> todos = await _repository.Todos
            .FindAll(trackChanges: false)
            .Select(t => new TodoResponseDto 
            { 
                Id = t.Id,
                Description = t.Description,
                CreatedOn = t.CreatedOn.ToShortDateString() + " " + t.CreatedOn.ToShortTimeString(),
                UpdatedOn = t.UpdatedOn.ToShortDateString() + " " + t.UpdatedOn.ToShortTimeString(),
                IsDone = t.IsDone
            })
            .ToListAsync();

        return todos;
    }

    public async Task UpdateTodo(long id, TodoUpdateDto updateDto)
    {
        var todo = await _repository.Todos.GetAsync(id);
        if (todo is null)
            throw new Exception("Todo doesn't exist");

        todo.Description = updateDto.Description;
        todo.IsDone = updateDto.IsDone;
        todo.UpdatedOn = DateTime.Now;

        await _repository.Todos.Update(todo);
        await _repository.SaveChanges();
    }

    public async Task<Todo> GetTodo(long id)
    {
        return await _repository.Todos.GetAsync(id);
    }

    
    /* Utility Functions */

    private async Task<ApplicationUser> GetLoggedInUserAsync()
    {
        var loggedInUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        return loggedInUser;
    }
}
