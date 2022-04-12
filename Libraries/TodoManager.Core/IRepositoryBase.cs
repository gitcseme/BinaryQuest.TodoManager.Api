using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TodoManager.Core;

public interface IRepositoryBase<TEntity, TKey>
{
    IQueryable<TEntity> FindAll(bool trackChanges);
	IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool trackChanges);
	Task Create(TEntity entity);
	Task Update(TEntity entity);
	Task Delete(TEntity entity);
	Task<TEntity> GetAsync(TKey id);
}
