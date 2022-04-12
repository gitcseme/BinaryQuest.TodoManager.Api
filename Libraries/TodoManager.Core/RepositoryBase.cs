using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TodoManager.Core;

public class RepositoryBase<TContext, TEntity, TKey> : IRepositoryBase<TEntity, TKey> 
	where TEntity : class
	where TContext : DbContext
{
	private readonly DbSet<TEntity> _dbSet;
	private readonly DbContext _dbContext;

	protected RepositoryBase(TContext context)
	{
		_dbContext = context;
		_dbSet = _dbContext.Set<TEntity>();
	}

	public IQueryable<TEntity> FindAll(bool trackChanges)
	{
		return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
	}

	public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool trackChanges)
	{
		return !trackChanges ? _dbSet.Where(expression).AsNoTracking() : _dbSet.Where(expression);
	}

	public async Task Create(TEntity entity)
	{
		await _dbSet.AddAsync(entity);
	}

	public async Task Update(TEntity entity)
	{
		await Task.Run(() =>
		{
			_dbSet.Attach(entity);
			_dbContext.Entry(entity).State = EntityState.Modified;
		});
	}

	public async Task Delete(TEntity entity)
	{
		await Task.Run(() =>
		{
			if (_dbContext.Entry(entity).State == EntityState.Detached)
				_dbSet.Attach(entity);

			_dbSet.Remove(entity);
		});
	}

    public async Task<TEntity> GetAsync(TKey id)
    {
		return await _dbSet.FindAsync(id);
    }
}
