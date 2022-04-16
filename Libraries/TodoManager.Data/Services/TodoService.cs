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
            CreatorId = (await GetLoggedInUserAsync()).Id,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now
        };

        await _repository.Todos.Create(todo);
        await _repository.SaveChanges();

        return PrepareTodoResponse(todo);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
    {
        var user = await GetLoggedInUserAsync();
        if (user is null)
            throw new Exception("User not found");

        IEnumerable<TodoResponseDto> todos = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id), trackChanges: false)
            .OrderByDescending(todo => todo.CreatedOn)
            .Select(t => PrepareTodoResponse(t))
            .ToListAsync();

        return todos;
    }

    public async Task UpdateTodo(long id, TodoUpdateDto updateDto)
    {
        var user = await GetLoggedInUserAsync();
        if (user is null)
            throw new Exception("User not found");

        var todoEntity = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id) && todo.Id.Equals(id), trackChanges: false)
            .FirstOrDefaultAsync();

        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        // Populate the update data
        todoEntity.Description = updateDto.Description;
        todoEntity.IsDone = updateDto.IsDone;
        todoEntity.UpdatedOn = DateTime.Now;
        todoEntity.Deadline = ConvertToDateTime(updateDto.Deadline);

        await _repository.Todos.Update(todoEntity);
        await _repository.SaveChanges();
    }

    public async Task<TodoResponseDto> GetTodo(long id)
    {
        var user = await GetLoggedInUserAsync();
        if (user is null)
            throw new Exception("User not found");

        var todoEntity = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id) && todo.Id.Equals(id), trackChanges: false)
            .FirstOrDefaultAsync();

        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        return PrepareTodoResponse(todoEntity);
    }

    public async Task Delete(long id)
    {
        var user = await GetLoggedInUserAsync();
        if (user is null)
            throw new Exception("User not found");

        var todoEntity = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id) && todo.Id.Equals(id), trackChanges: false)
            .FirstOrDefaultAsync();

        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        await _repository.Todos.Delete(todoEntity);
        await _repository.SaveChanges();
    }

    public async Task<IEnumerable<TodoResponseDto>> Search(string? searchText)
    {
        var user = await GetLoggedInUserAsync();
        if (user is null)
            throw new Exception("User not found");

        var todoResponses = _repository.Todos;

        if (searchText == "" || searchText == null)
        {
            return await todoResponses.Find(t =>
                t.CreatorId.Equals(user.Id), trackChanges: false)
            .OrderByDescending(t => t.CreatedOn)
            .Select(t => PrepareTodoResponse(t))
            .ToListAsync();
        }

        return await todoResponses.Find(t =>
                t.CreatorId.Equals(user.Id) &&
                t.Description.ToLower().Contains(searchText.ToLower()), trackChanges: false)
            .OrderByDescending(t => t.CreatedOn)
            .Select(t => PrepareTodoResponse(t))
            .ToListAsync();
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
            IsDone = todo.IsDone,
            CreatedOn = todo.CreatedOn.ToShortDateString() + " " + todo.CreatedOn.ToShortTimeString(),
            UpdatedOn = todo.UpdatedOn.ToShortDateString() + " " + todo.UpdatedOn.ToShortTimeString(),
            Deadline = todo.Deadline?.ToShortDateString()
        };
    }

    private static DateTime? ConvertToDateTime(long? deadline)
    {
        if (deadline is null)
            return null;

        var date = new DateTime(1970, 1, 1, 0, 0, 0, 0); // epoch start
        date = date.AddMilliseconds((double)deadline);
        return date;
    }

}
