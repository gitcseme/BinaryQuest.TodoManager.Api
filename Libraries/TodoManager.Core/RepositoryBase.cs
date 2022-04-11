using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TodoManager.Core;

public class RepositoryBase<TContext, TEntity> : IRepositoryBase<TEntity> 
	where TEntity : class
	where TContext : DbContext
{
	private readonly DbSet<TEntity> _dbSet;

	protected RepositoryBase(TContext context)
	{
		_dbSet = context.Set<TEntity>();
	}

	public IQueryable<TEntity> FindAll(bool trackChanges)
	{
		return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
	}

	public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool trackChanges)
	{
		return !trackChanges ? _dbSet.Where(expression).AsNoTracking() : _dbSet.Where(expression);
	}

	public void Create(TEntity entity)
	{
		_dbSet.Add(entity);
	}

	public void Update(TEntity entity)
	{
		_dbSet.Update(entity);
	}

	public void Delete(TEntity entity)
	{
		_dbSet.Remove(entity);
	}
}
