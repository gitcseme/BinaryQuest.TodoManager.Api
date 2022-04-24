using TodoManager.Data.Entities;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Data.Services;

public interface ITodoService
{
    Task<TodoResponseDto> CreateTodoAsync(TodoCreateDto createDto);
    Task<IEnumerable<TodoResponseDto>> GetAllAsync();
    Task UpdateTodoAsync(long id, TodoUpdateDto updateDto);
    Task<TodoResponseDto> GetByIdAsync(long id);
    Task DeleteAsync(long id);
    Task<IEnumerable<TodoResponseDto>> SearchAsync(string? searchText);
}
