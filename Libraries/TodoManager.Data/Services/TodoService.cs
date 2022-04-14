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

    public async Task<TodoResponseDto> CreateTodo(TodoCreateDto createDto)
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

        return PrepareTodoResponse(todo);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
    {
        IEnumerable<TodoResponseDto> todos = await _repository.Todos
            .FindAll(trackChanges: false)
            .Select(t => PrepareTodoResponse(t))
            .ToListAsync();

        return todos;
    }

    public async Task UpdateTodo(long id, TodoUpdateDto updateDto)
    {
        var todoEntity = await _repository.Todos.GetAsync(id);
        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        todoEntity.Description = updateDto.Description;
        todoEntity.IsDone = updateDto.IsDone;
        todoEntity.UpdatedOn = DateTime.Now;

        await _repository.Todos.Update(todoEntity);
        await _repository.SaveChanges();
    }

    public async Task<TodoResponseDto> GetTodo(long id)
    {
        var todoEntity = await _repository.Todos.GetAsync(id);
        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        return PrepareTodoResponse(todoEntity);
    }

    
    /* Utility Functions */

    private async Task<ApplicationUser> GetLoggedInUserAsync()
    {
        var loggedInUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        return loggedInUser;
    }

    // todo: replace with automapper
    private static TodoResponseDto PrepareTodoResponse(Todo todo)
    {
        return new TodoResponseDto
        {
            Id = todo.Id,
            Description = todo.Description,
            IsDone = false,
            CreatedOn = todo.CreatedOn.ToShortDateString() + " " + todo.CreatedOn.ToShortTimeString(),
            UpdatedOn = todo.UpdatedOn.ToShortDateString() + " " + todo.UpdatedOn.ToShortTimeString(),
        };
    }
}
