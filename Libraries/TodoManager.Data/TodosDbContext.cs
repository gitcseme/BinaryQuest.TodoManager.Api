using Microsoft.EntityFrameworkCore;
using TodoManager.Data.Entities;

namespace TodoManager.Data;

public class TodosDbContext : DbContext
{
    public TodosDbContext(DbContextOptions<TodosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Todo>()
            .Property(t => t.CreatorId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Todo> Todos { get; set; }
}
