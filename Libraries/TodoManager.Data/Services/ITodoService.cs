using TodoManager.Data.Entities;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Data.Services;

public interface ITodoService
{
    Task<TodoResponseDto> CreateTodo(TodoCreateDto createDto);
    Task<IEnumerable<TodoResponseDto>> GetAllAsync();
    Task UpdateTodo(long id, TodoUpdateDto updateDto);
    Task<TodoResponseDto> GetTodo(long id);
    Task Delete(long id);
}
