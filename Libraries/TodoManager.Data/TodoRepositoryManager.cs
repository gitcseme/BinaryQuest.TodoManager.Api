using TodoManager.Data.Repositories;

namespace TodoManager.Data;

public class TodoRepositoryManager : ITodoRepositoryManager
{
    private readonly TodosDbContext _todoContext;

    public TodoRepositoryManager(TodosDbContext todoContext)
    {
        _todoContext = todoContext;
        Todos = new TodoRepository(_todoContext);
    }

    public async Task SaveChanges() => await _todoContext.SaveChangesAsync();

    public ITodoRepository Todos { get; set; }
}
