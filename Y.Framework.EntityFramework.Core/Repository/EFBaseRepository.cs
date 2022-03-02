using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Y.Framework.EntityFramework.Core.Entity;
using Y.Framework.EntityFramework.Core.Model;
using Y.Framework.EntityFramework.Core.Specification;
using Y.Framework.Extensions;

namespace Y.Framework.EntityFramework.Core.Repository;

public class EFBaseRepository<T> : IEFBaseRepository<T> where T : BaseEntity
{
    readonly Expression<Func<T, bool>> _fixedExpression = x => x.IsDeleted == false;

    readonly DbContext _DbContext;

    public EFBaseRepository(DbContext context)
    {
        _DbContext = context;
    }

    public async Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _DbContext.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public async Task<T?> GetSingleOrDefaultAsync(BaseSpecification<T> spec)
    {
        return await ApplySpecification(spec).SingleOrDefaultAsync();
    }

    public async Task<T?> GetLastOrDefaultAsync(BaseSpecification<T> spec)
    {
        CheckOrderByList(spec);
        return await ApplySpecification(spec).LastOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(BaseSpecification<T> spec)
    {
        CheckOrderByList(spec);
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
    {
        predicate = predicate.And(_fixedExpression);
        return await _DbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<List<T>> GetListAsync(BaseSpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<PageResultModel<T>> GetPagedListAsync(PagedSpecification<T> spec)
    {
        var count = await GetCountAsync(spec.Criteria);
        var list = await GetListAsync(spec);
        return new PageResultModel<T>(spec.PageIndex, spec.PageSize, count, list);
    }

    public async Task AddAsync(T entity)
    {
        await _DbContext.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await _DbContext.Set<T>().AddRangeAsync(entities);
    }

    public async Task RemoveAsync(T entity)
    {
        _DbContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task RemoveRangeAsync(List<T> entities)
    {
        _DbContext.Set<T>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(T entity)
    {
        _DbContext.Set<T>().Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(List<T> entities)
    {
        _DbContext.Set<T>().UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
    {
        predicate = predicate.And(_fixedExpression);
        return await _DbContext.Set<T>().CountAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _DbContext.Set<T>().AnyAsync(predicate);
    }

    public async Task<bool> SaveChangeAsync()
    {
        return await _DbContext.SaveChangesAsync() > 0;
    }

    private IQueryable<T> ApplySpecification(BaseSpecification<T> specification, bool ifGetCount = false)
    {
        var query = _DbContext.Set<T>().AsQueryable();
        query = query.Where(_fixedExpression);
        query = query.Where(specification.Criteria);
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
        if (specification.OrderByList.Any())
        {
            IOrderedQueryable<T>? orderQuery = null;
            foreach (var item in specification.OrderByList)
            {
                if (item.OrderBy == null) continue;
                if (orderQuery == null)
                {
                    orderQuery = item.IsDescending ? query.OrderByDescending(item.OrderBy) : query.OrderBy(item.OrderBy);
                }
                else
                {
                    orderQuery = item.IsDescending ? orderQuery.ThenByDescending(item.OrderBy) : orderQuery.ThenBy(item.OrderBy);
                }
            }

            if (orderQuery != null)
            {
                query = orderQuery;
            }
        }
        if (!ifGetCount && specification is PagedSpecification<T> pagedSpecification)
        {
            query = query.Skip((pagedSpecification.PageIndex - 1) * pagedSpecification.PageSize).Take(pagedSpecification.PageSize);
        }
        return query;
    }

    private void CheckOrderByList(BaseSpecification<T> specification)
    {
        if (!specification.OrderByList.Any())
        {
            throw new ArgumentNullException("Order By List cannot be null when using Last or First.");
        }
    }
}