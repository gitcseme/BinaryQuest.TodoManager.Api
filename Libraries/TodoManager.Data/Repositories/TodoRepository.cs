using Microsoft.EntityFrameworkCore;
using TodoManager.Core;
using TodoManager.Data.Entities;

namespace TodoManager.Data.Repositories;

public class TodoRepository : RepositoryBase<TodosDbContext, Todo, long>, ITodoRepository
{
    public TodoRepository(TodosDbContext context) : base(context)
    {
    }

    public async Task<Todo> GetByIdAsync(long userId, long todoId)
    {
        return await Find(todo => todo.CreatorId.Equals(userId) && todo.Id.Equals(todoId), trackChanges: false)
            .SingleAsync();
    }
}
