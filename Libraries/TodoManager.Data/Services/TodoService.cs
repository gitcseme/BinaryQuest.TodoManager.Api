using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoManager.Data.Entities;
using TodoManager.Shared.Entities;
using TodoManager.Shared.CustomExceptions;
using TodoManager.Shared.TodoDtos;
using TodoManager.Shared.Services;

namespace TodoManager.Data.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepositoryManager _repository;
    private IUtilityService _utilityService;


    public TodoService(
        ITodoRepositoryManager repository,
        IUtilityService utilityService)
    {
        _repository = repository;
        _utilityService = utilityService;
    }

    public async Task<TodoResponseDto> CreateTodoAsync(TodoCreateDto createDto)
    {
        var todo = new Todo()
        {
            Description = createDto.Description,
            IsDone = false,
            CreatorId = (await _utilityService.GetLoggedInUserAsync()).Id,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now
        };

        await _repository.Todos.Create(todo);
        await _repository.SaveChanges();

        return PrepareTodoResponse(todo);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
    {
        var user = await _utilityService.GetLoggedInUserAsync();

        IEnumerable<TodoResponseDto> todos = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id), trackChanges: false)
            .OrderByDescending(todo => todo.CreatedOn)
            .Select(t => PrepareTodoResponse(t))
            .ToListAsync();

        return todos;
    }

    public async Task UpdateTodoAsync(long id, TodoUpdateDto updateDto)
    {
        var user = await _utilityService.GetLoggedInUserAsync();

        var todoEntity = await _repository.Todos
            .Find(todo => todo.CreatorId.Equals(user.Id) && todo.Id.Equals(id), trackChanges: false)
            .FirstOrDefaultAsync();

        if (todoEntity is null)
            throw new ApiException("Todo doesn't exists", StatusCodes.Status500InternalServerError);

        // Populate the update data
        todoEntity.Description = updateDto.Description;
        todoEntity.IsDone = updateDto.IsDone;
        todoEntity.UpdatedOn = DateTime.Now;
        todoEntity.Deadline = _utilityService.ConvertToDateTime(updateDto.Deadline);

        await _repository.Todos.Update(todoEntity);
        await _repository.SaveChanges();
    }

    public async Task<TodoResponseDto> GetByIdAsync(long id)
    {
        var user = await _utilityService.GetLoggedInUserAsync();

        var todoEntity = await _repository.Todos.GetByIdAsync(user.Id, id);

        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        return PrepareTodoResponse(todoEntity);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _utilityService.GetLoggedInUserAsync();

        var todoEntity = await _repository.Todos.GetByIdAsync(user.Id, id);

        if (todoEntity is null)
            throw new Exception("Todo doesn't exists");

        await _repository.Todos.Delete(todoEntity);
        await _repository.SaveChanges();
    }

    public async Task<IEnumerable<TodoResponseDto>> SearchAsync(string? searchText)
    {
        var user = await _utilityService.GetLoggedInUserAsync();

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

    #region Utility Functions
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
    #endregion

}
