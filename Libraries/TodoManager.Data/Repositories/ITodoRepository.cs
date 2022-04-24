using TodoManager.Core;
using TodoManager.Data.Entities;

namespace TodoManager.Data.Repositories;

public interface ITodoRepository : IRepositoryBase<Todo, long>
{
    Task<Todo> GetByIdAsync(long userId, long todoId);
}
