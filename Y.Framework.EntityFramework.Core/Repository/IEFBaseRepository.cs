using System.Linq.Expressions;
using Y.Framework.EntityFramework.Core.Entity;
using Y.Framework.EntityFramework.Core.Model;
using Y.Framework.EntityFramework.Core.Specification;

namespace Y.Framework.EntityFramework.Core.Repository;

public interface IEFBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetSingleOrDefaultAsync(BaseSpecification<T> spec);

    Task<T?> GetLastOrDefaultAsync(BaseSpecification<T> spec);
    Task<T?> GetFirstOrDefaultAsync(BaseSpecification<T> spec);

    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetListAsync(BaseSpecification<T> spec);
    Task<PageResultModel<T>> GetPagedListAsync(PagedSpecification<T> spec);

    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entities);

    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(List<T> entities);

    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(List<T> entities);

    Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    Task<bool> SaveChangeAsync();
}