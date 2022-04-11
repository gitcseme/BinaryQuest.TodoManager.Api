using TodoManager.Data.Repositories;

namespace TodoManager.Data;

public interface ITodoRepositoryManager
{
    ITodoRepository Todos { get; }

    Task SaveChanges();
}
