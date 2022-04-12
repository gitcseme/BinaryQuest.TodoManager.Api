using TodoManager.Core;
using TodoManager.Data.Entities;

namespace TodoManager.Data.Repositories;

public class TodoRepository : RepositoryBase<TodosDbContext, Todo, long>, ITodoRepository
{
    public TodoRepository(TodosDbContext context) : base(context)
    {
    }
}
