using System.Diagnostics;
using System.Linq.Expressions;

namespace Y.Framework.Extensions;

public static class ExpressionExtensions
{
    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        // build parameter map (from parameters of second to parameters of first)
        Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

        // replace parameters in the second lambda expression with parameters from the first
        var secondBody = ParameterRebinder.ReplaceParameters(map!, second.Body);
        // apply composition of lambda expression bodies to parameters from the first expression 
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.And);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.Or);
    }
}

class ParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression?> _map;

    public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression?> map)
    {
        this._map = map;
    }

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression?> map, Expression exp)
    {
        return new ParameterRebinder(map).Visit(exp);
    }

    protected override Expression VisitParameter(ParameterExpression? node)
    {
        if (node != null && this._map.TryGetValue(node, out ParameterExpression? replacement))
            node = replacement;
        Debug.Assert(node != null, nameof(node) + " != null");
        return base.VisitParameter(node);
    }
}