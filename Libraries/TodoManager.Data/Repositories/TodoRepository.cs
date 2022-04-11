using TodoManager.Core;
using TodoManager.Data.Entities;

namespace TodoManager.Data.Repositories;

public class TodoRepository : RepositoryBase<TodosDbContext, Todo>, ITodoRepository
{
    protected TodoRepository(TodosDbContext context) : base(context)
    {
    }
}
