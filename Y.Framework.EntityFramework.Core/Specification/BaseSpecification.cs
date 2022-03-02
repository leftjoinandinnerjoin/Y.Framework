using System.Linq.Expressions;
using Y.Framework.Extensions;

namespace Y.Framework.EntityFramework.Core.Specification;

public class BaseSpecification<T>
{
    protected internal Expression<Func<T, bool>> Criteria { get; private set; } = x => true;
    protected internal List<Expression<Func<T, object>>> Includes { get; } = new();
    protected internal List<string> IncludeStrings { get; } = new();
    public List<OrderByList<T>> OrderByList { get; } = new();
    public void AddPredicate(Expression<Func<T, bool>> predicateExpression)
    {
        Criteria = Criteria.And(predicateExpression);
    }

    public void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
    public void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
    public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderByList.Add(new OrderByList<T>() { OrderBy = orderByExpression, IsDescending = false });
    }
    public void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByList.Add(new OrderByList<T>() { OrderBy = orderByDescendingExpression, IsDescending = true });
    }
}