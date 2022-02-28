namespace Y.Framework.EntityFramework.Core.Specification;

public class Specification<T> : BaseSpecification<T>
{
    public static Specification<T> GetSpecification()
    {
        return new Specification<T>();
    }
}