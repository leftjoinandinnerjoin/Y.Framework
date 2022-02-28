using System.Linq.Expressions;

namespace Y.Framework.EntityFramework.Core.Specification;

public class OrderByList<T>
{
    public Expression<Func<T, object>>? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}