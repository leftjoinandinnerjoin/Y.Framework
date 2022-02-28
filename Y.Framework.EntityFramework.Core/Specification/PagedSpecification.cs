using Y.Framework.EntityFramework.Core.Model;

namespace Y.Framework.EntityFramework.Core.Specification;

public class PagedSpecification<T> : BaseSpecification<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    PagedSpecification(PageQueryModel queryModel)
    {
        PageIndex = queryModel.PageIndex;
        PageSize = queryModel.PageSize;
    }
    public static PagedSpecification<T> GetSpecification(PageQueryModel queryModel)
    {
        return new PagedSpecification<T>(queryModel);
    }
}